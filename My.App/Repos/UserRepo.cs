using Microsoft.EntityFrameworkCore;
using My.Database.Models;
using My.Database.Context;
using My.App.Interfaces;
using AutoMapper;
using My.App.Dtos;

namespace My.App.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper? _mapper;

        public UserRepo(
            MyDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = null;
        }

        public UserRepo(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }




        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<UserDto>> GetAllUserDtosAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }



        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public async Task<UserDto> GetUserDtoByIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            return _mapper.Map<UserDto>(user);
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

        public async Task<List<UserDto>> GetUserDtosByCreatedAtAsync(DateTime startDate, DateTime endDate)
        {
            var users = await _dbContext.Users
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();

            if (!users.Any())
            {
                throw new KeyNotFoundException($"No users found between {startDate} and {endDate}.");
            }

            return _mapper.Map<List<UserDto>>(users);
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

        public async Task<UserDto> InsertUserDtoAsync(UserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var lastId = await _dbContext.Users.OrderByDescending(d => d.Id).FirstOrDefaultAsync();
            if (lastId != null)
            {
                user.Id = lastId.Id + 1;
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
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

        public async Task<UserDto> UpdateUserDtoAsync(UserDto dto)
        {
            var existingUser = await _dbContext.Users.FindAsync(dto.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {dto.Id} not found.");
            }

            // Update the entity with values from the DTO
            _mapper.Map(dto, existingUser);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(existingUser);
        }

       
       
    }
}