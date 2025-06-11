using Microsoft.EntityFrameworkCore;
using My.Database.Context;
using My.App.Repos;
using My.Database.Models;
using AutoMapper;
using My.App.Dtos;

namespace My.App.Tests
{
    public class UserRepoTests : IDisposable
    {
        private readonly string _tempDbPath;
        private readonly MyDbContext _dbContext;
        private readonly UserRepo _repo;



        public UserRepoTests()
        {
            // Create temp DB file
            var originalDbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "My.Database", "app.db");
            _tempDbPath = Path.GetTempFileName();
            File.Copy(originalDbPath, _tempDbPath, overwrite: true);

            // Setup DbContext using temp DB file
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlite($"Data Source={_tempDbPath}")
                .Options;

            _dbContext = new MyDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _repo = new UserRepo(_dbContext);
        }

        [Fact]
        public async Task GetAllUsersAsync()
        {
            // Act
            var users = await _repo.GetAllUsersAsync();

            // Assert
            Assert.NotNull(users);
            Assert.NotEmpty(users);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UserRepoTests2.GetAllUsersAsync"

        [Fact]
        public async Task GetUserByIdAsync()
        {
            // Act
            var user = await _repo.GetUserByIdAsync(1);

            // Assert
            Assert.Equal("admin@example.com", user.Email);
            Assert.Equal(UserRole.Admin, user.Role);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UserRepoTests2.GetUserByIdAsync"

        [Fact]
        public async Task InsertUserAsync()
        {
            // Arrange
            var user = new User
            {
                Email = "test-user@example.com",
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var addedUser = await _repo.InsertUserAsync(user);

            // Assert
            Assert.True(addedUser.Id > 1);
            Assert.Equal("test-user@example.com", addedUser.Email);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UserRepoTests2.InsertUserAsync"

        [Fact]
        public async Task UpdateUserAsync()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Email = "update-user@example.com",
                FirstName = "Update",
                LastName = "User",
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var addedUser = await _repo.UpdateUserAsync(user);

            // Assert
            Assert.Equal("update-user@example.com", addedUser.Email);
        } // dotnet test --filter "FullyQualifiedName=My.App.Tests.UserRepoTests2.UpdateUserAsync"

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Database.CloseConnection();
                _dbContext.Dispose();
            }

            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    if (File.Exists(_tempDbPath))
                    {
                        File.Delete(_tempDbPath);
                    }
                    break;  // success
                }
                catch (IOException)
                {
                    retries--;
                    System.Threading.Thread.Sleep(100);  // wait 100 ms before retrying
                }
            }
        }
        


    }
}
