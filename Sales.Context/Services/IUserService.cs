using Sales.Library;

namespace Sales.Context.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> DeleteUserByIdAsync(string id);
        Task<User?> GetUserByIdAsync(string id);
        Task<IEnumerable<User>?> GetUsersAsync();
        Task<bool> UpdateUserAsync(User user);
    }
}
