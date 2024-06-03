using Sales.Client.Models;
using Sales.Library;

namespace Sales.Client.Repositories
{
    public interface IAccountRepository
    {
        Task<List<User>?> GetUsersAsync();
        Task<bool> LoginAsync(LoginModel loginModel);
        Task<bool> RegisterAsync(RegisterModel register);
    }
}
