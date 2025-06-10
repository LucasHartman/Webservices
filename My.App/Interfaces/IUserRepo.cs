using My.Database.Models;

namespace My.App.Interfaces
{
    public interface IUserRepo
    {
        Task<IReadOnlyList<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task<List<User>> GetUsersByCreatedAtAsync(DateTime startDate, DateTime endDate);

        Task<User> InsertUserAsync(User User);

        Task<User> UpdateUserAsync(User User);
    }
}
