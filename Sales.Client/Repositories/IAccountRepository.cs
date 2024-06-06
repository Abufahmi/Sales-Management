using Sales.Client.Models;
using Sales.Library;
using Sales.Library.Models;

namespace Sales.Client.Repositories
{
    public interface IAccountRepository
    {
        Task<TokenModel?> GetRefreshTokenAsync(RefreshTokenModel refreshToken);
        Task<List<User>?> GetUsersAsync();
        Task<bool> LoginAsync(LoginModel loginModel);
        Task<bool> LogoutAsync();
        Task<bool> RegisterAsync(RegisterModel register);
        Task<string?> TestAuthenticationAsync();
    }
}
