using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My.Database.Context;
using My.Database.Models;
using My.App.Controllers;
using My.App.Repos;
using My.App.Dtos;
using AutoMapper;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace My.App.Tests
{
    public class UsersControllerTests : IDisposable
    {
        private readonly string _tempDbPath;
        private readonly MyDbContext _dbContext;
        private readonly UserRepo _userRepo;
        private readonly UsersController _controller;
        private readonly IMapper _mapper;

        public UsersControllerTests()
        {
            // AutoMapper config
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            // Copy temp database
            var originalDbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "My.Database", "app.db");
            _tempDbPath = Path.GetTempFileName();
            File.Copy(originalDbPath, _tempDbPath, overwrite: true);

            // Setup DbContext
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlite($"Data Source={_tempDbPath}")
                .Options;

            _dbContext = new MyDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _userRepo = new UserRepo(_dbContext, _mapper);
            _controller = new UsersController(_userRepo);
        }

        [Fact]
        public async Task GetAllUsers()
        {
            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.NotEmpty(users);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UsersControllerTests.GetAllUsers"

        [Fact]
        public async Task GetUserById()
        {
            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(1, user.Id);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UsersControllerTests.GetUserById"


        [Fact]
        public async Task GetUsersByCreatedAt()
        {
            // Arrange
            var start = new DateTime(2020, 1, 1);
            var end = new DateTime(2030, 1, 1);

            // Act
            var result = await _controller.GetUsersByCreatedAt(start, end);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.NotEmpty(users);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UsersControllerTests.GetUsersByCreatedAt"

        [Fact]
        public async Task InsertUser()
        {
            // Arrange
            var dto = new UserDto
            {
                Email = "new@example.com",
                FirstName = "New",
                LastName = "User",
                Role = UserRole.User
            };

            // Act
            var result = await _controller.InsertUser(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var newUser = Assert.IsType<UserDto>(createdResult.Value);
            Assert.Equal(dto.Email, newUser.Email);
            Assert.True(newUser.Id > 0);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UsersControllerTests.InsertUser"

        [Fact]
        public async Task UpdateUser()
        {
            // Arrange
            var dto = new UserDto
            {
                Email = "update@example.com",
                FirstName = "Update",
                LastName = "Me",
                Role = UserRole.User
            };

            var inserted = await _userRepo.InsertUserDtoAsync(dto);
            inserted.FirstName = "Updated";

            // Act
            var result = await _controller.UpdateUser(inserted.Id, inserted);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updated = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal("Updated", updated.FirstName);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UsersControllerTests.UpdateUser"

    

    

        public void Dispose()
        {
            _dbContext?.Database.CloseConnection();
            _dbContext?.Dispose();

            int retries = 5;
            while (retries-- > 0)
            {
                try
                {
                    if (File.Exists(_tempDbPath))
                    {
                        File.Delete(_tempDbPath);
                    }
                    break;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
