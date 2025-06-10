using AutoMapper;
using My.Database.Models;
using My.App.Dtos;
using My.App.Interfaces;

namespace My.App.Services;

public class UserService : IUserService
{
    private readonly IUserRepo _UserRepo;
    private readonly IMapper _mapper;

    public UserService(IUserRepo UserRepo, IMapper mapper)
    {
        _UserRepo = UserRepo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<UserIndexDto>> GetAllUsersAsync()
    {
        var Users = await _UserRepo.GetAllUsersAsync();        
        var UsersDto = _mapper.Map<IReadOnlyList<UserIndexDto>>(Users);
        return UsersDto;
    }

    public async Task<UserFormDto> GetUserByIdAsync(int id)
    {
        var User = await _UserRepo.GetUserByIdAsync(id);
        var UserDto = _mapper.Map<UserFormDto>(User);
        return UserDto;
    }

    public async Task<IReadOnlyList<UserIndexDto>> GetUsersByCreatedAtAsync(DateTime startDate, DateTime endDate)
    {
        var Users = await _UserRepo.GetUsersByCreatedAtAsync(startDate, endDate);
        var UsersDto = _mapper.Map<IReadOnlyList<UserIndexDto>>(Users);        
       return UsersDto;
    }

    public async Task<UserFormDto> InsertUserAsync(UserFormDto UserDto)
    {
        var User = _mapper.Map<User>(UserDto);
        var insertedUser = await _UserRepo.InsertUserAsync(User);
        var resultDto = _mapper.Map<UserFormDto>(insertedUser);
        return resultDto;
    }

    public async Task<UserFormDto> UpdateUserAsync(int id, UserFormDto UserDto)
    {
        var existingUser = await _UserRepo.GetUserByIdAsync(id);
        
        if (existingUser == null)
        {
            throw new Exception($"User with ID {id} not found.");
        }
        _mapper.Map(UserDto, existingUser);

        var updatedUser = await _UserRepo.UpdateUserAsync(existingUser);
        var resultDto = _mapper.Map<UserFormDto>(updatedUser);
        
        return resultDto;
    }
}
