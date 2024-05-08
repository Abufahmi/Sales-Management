using Sales.Library;

namespace Sales.Context.Services
{
    public interface IDbService
    {
        Task<bool> CreateLoginAsync(string userId, string refreshToken);
        Task<bool> CreateRolesAsync();
        Task<bool> CreateUserRoleIfNotExistsAsync(User user);
        string GenerateRefreshToken();
        string GenerateToken(User user, string roleName);
        Task<string?> GetRoleNameByUserIdAsync(string userId);
    }
}
