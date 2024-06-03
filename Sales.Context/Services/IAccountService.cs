using Sales.Library;
using Sales.Library.Models;
using static Sales.Context.Helpers.Responses;

namespace Sales.Context.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<LoginResponse> LoginAsync(Login login);
        Task<DefaultResponse> RegisterAsync(Register register);
    }
}
