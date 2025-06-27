using Sales.Library.Entities;

namespace Sales.Context.Contracts
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> DeleteUserByIdAsync(string id);
        Task<User?> GetUserByIdAsync(string id);
        Task<IEnumerable<User>?> GetUsersAsync();
        Task<bool> IsUserExestAsync(string id);
        Task<bool> UpdateUserAsync(User user);
    }
}
