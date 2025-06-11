using My.App.Dtos;
using My.Database.Models;

namespace My.App.Interfaces
{
    public interface IUserRepo
    {
        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task<IReadOnlyList<UserDto>> GetAllUserDtosAsync();


        Task<User> GetUserByIdAsync(int id);
        Task<UserDto> GetUserDtoByIdAsync(int id);


        Task<List<User>> GetUsersByCreatedAtAsync(DateTime startDate, DateTime endDate);
        Task<List<UserDto>> GetUserDtosByCreatedAtAsync(DateTime startDate, DateTime endDate);


        Task<User> InsertUserAsync(User user);
        Task<UserDto> InsertUserDtoAsync(UserDto dto);


        Task<User> UpdateUserAsync(User user);
        Task<UserDto> UpdateUserDtoAsync(UserDto dto);
    }
}
