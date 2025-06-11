using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using My.Database.Context;
using My.Database.Models;
using My.App.Repos;

namespace My.App.Tests
{
    public class UserRepoTests
    {
        private MyDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            var dbContext = new MyDbContext(options);

            // Seed data if needed
            dbContext.Users.AddRange(
                new User { Id = 1, FirstName = "Alice", LastName = "Flower", Email = "alice.flower@example.me", CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new User { Id = 2, FirstName = "Bob", LastName = "Rock", Email = "bob.rock@example.me", CreatedAt = DateTime.UtcNow }
            );
            dbContext.SaveChanges();

            return dbContext;
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var repo = new UserRepo(dbContext);

            // Act
            var result = await repo.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsCorrectUser()
        {
            var dbContext = GetInMemoryDbContext();
            var repo = new UserRepo(dbContext);

            var user = await repo.GetUserByIdAsync(1);
            Assert.Equal("Alice", user.FirstName);
        }

        [Fact]
        public async Task InsertUserAsync_AddsUserCorrectly()
        {
            var dbContext = GetInMemoryDbContext();
            var repo = new UserRepo(dbContext);

            var newUser = new User
            {
                FirstName = "Charlie",
                LastName = "Cloud",
                Email = "charlie-cloud@example.me",
                CreatedAt = DateTime.UtcNow
            };

            var addedUser = await repo.InsertUserAsync(newUser);

            Assert.Equal(3, addedUser.Id); // assuming 2 users already seeded
            Assert.Equal("Charlie", addedUser.FirstName);
        }
    }
}
