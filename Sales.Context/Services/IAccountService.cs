using Sales.Library;
using Sales.Library.Models;
using static Sales.Context.Helpers.Responses;

namespace Sales.Context.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<LoginResponse> LoginAsync(Login login);
        Task<TokenModel?> RefreshTokenAsync(string refreshToken);
        Task<DefaultResponse> RegisterAsync(Register register);
    }
}
