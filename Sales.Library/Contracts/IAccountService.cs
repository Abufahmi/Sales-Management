using Sales.Library.Entities;
using Sales.Library.Models;

namespace Sales.Library.Contracts;

public interface IAccountService
{
    Task<string?> ForgetPasswordAsync(ForgetPassword forgetPassword);
    Task<MainSetting?> GetMainSettingAsync();
    Task<TokenModel?> GetRefreshTokenAsync(RefreshTokenModel refreshToken);
    Task<List<User>?> GetUsersAsync();
    Task<bool> IsTokenExistsAsync(string token);
    Task<bool> LoginAsync(Login login);
    Task<bool> LogoutAsync();
    Task<bool> RegisterAsync(Register register);
    Task<bool> ResetPasswordAsync(ResetAccount reset);
    Task<string?> VerifyCodeAsync(string? verificationCode);
}
