using Sales.Client.Models;
using Sales.Library;
using Sales.Library.Models;

namespace Sales.Client.Repositories
{
    public interface IAccountRepository
    {
        Task<string?> ForgetPasswordAsync(ForgetPassword forgetPassword);
        Task<MainSetting?> GetMainSettingAsync();
        Task<TokenModel?> GetRefreshTokenAsync(RefreshTokenModel refreshToken);
        Task<List<User>?> GetUsersAsync();
        Task<bool> IsTokenExistsAsync(string token);
        Task<bool> LoginAsync(LoginModel loginModel);
        Task<bool> LogoutAsync();
        Task<bool> RegisterAsync(RegisterModel register);
        Task<bool> ResetPasswordAsync(ResetAccount reset);
        Task<string?> VerifyCodeAsync(string? verificationCode);
    }
}
