using My.App.Dtos;

namespace My.App.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<UserIndexDto>> GetAllUsersAsync();
    Task<UserFormDto> GetUserByIdAsync(int id);
    Task<IReadOnlyList<UserIndexDto>> GetUsersByCreatedAtAsync(DateTime startDate, DateTime endDate);
    Task<UserFormDto> InsertUserAsync(UserFormDto UserDto);
    Task<UserFormDto> UpdateUserAsync(int id, UserFormDto UserDto);
}