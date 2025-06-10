using Microsoft.EntityFrameworkCore;
using My.Database.Models;
using My.Database.Context;
using My.App.Interfaces;

namespace My.App.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly MyDbContext _dbContext;

        public UserRepo(
            MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public async Task<List<User>> GetUsersByCreatedAtAsync(DateTime startDate, DateTime endDate)
        {
            var Users = await _dbContext.Users
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
            
            if (!Users.Any())
            {
                throw new KeyNotFoundException($"No Users found between {startDate} and {endDate}.");
            }
            
            return Users;
        }

        public async Task<User> InsertUserAsync(User User)
        {
            // Retrieve the last UserNumber
            var lastId = await _dbContext.Users
                .OrderByDescending(d => d.Id)
                .FirstOrDefaultAsync();

            // Increment Id
            if (lastId != null)
            {
                User.Id = lastId.Id + 1;
            }
 
            // Add the new User and save changes
            await _dbContext.Users.AddAsync(User);
            await _dbContext.SaveChangesAsync();

            return User;
        }

        public async Task<User> UpdateUserAsync(User User)
        {
            var existingItem = await _dbContext.Users.FindAsync(User.Id);
            if (existingItem != null)
            {
                _dbContext.Entry(existingItem).CurrentValues.SetValues(User);
                await _dbContext.SaveChangesAsync();
            }
            return User;
        }
    }
}