using Sales.Library;

namespace Sales.Context.Services
{
    public interface IUserRoleService
    {
        Task<bool> CreateUserRoleAsync(UserRole userRole);
        Task<bool> DeleteUserRoleByIdAsync(string id);
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<UserRole?> GetUserRoleByIdAsync(string id);
        Task<IEnumerable<UserRole>> GetUserRolesAsync();
        Task<bool> UpdateUserRoleAsync(UserRole userRole);
    }
}
