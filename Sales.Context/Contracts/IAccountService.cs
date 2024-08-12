using Sales.Library;
using Sales.Library.Models;
using static Sales.Context.Helpers.Responses;

namespace Sales.Context.Contracts
{
    public interface IAccountService
    {
        Task<LoginResponse> ForgetPasswordAsync(string email);
        Task<MainSetting?> GetMainSettingAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<DefaultResponse> IsTokenExistsAsync(string token);
        Task<LoginResponse> LoginAsync(Login login);
        Task<TokenModel?> RefreshTokenAsync(string refreshToken);
        Task<DefaultResponse> RegisterAsync(Register register);
        Task<bool> ResetAccountAsync(ResetAccount resetAccount);
        Task<string?> VerifyCodeAsync(string verificationCode);
    }
}
