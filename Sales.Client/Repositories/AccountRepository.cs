using Microsoft.AspNetCore.Components.Authorization;
using Sales.Client.Helpers;
using Sales.Client.Models;
using Sales.Client.Services;
using Sales.Library;
using Sales.Library.Models;
using System.Net.Http.Json;

namespace Sales.Client.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ClientService clientService;
        private readonly AuthenticationStateProvider stateProvider;

        public AccountRepository(ClientService clientService, AuthenticationStateProvider stateProvider)
        {
            this.clientService = clientService;
            this.stateProvider = stateProvider;
        }

        public async Task<List<User>?> GetUsersAsync()
        {
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            try
            {
                HttpResponseMessage result = await httpClient.GetAsync("Account/GetUsers");
                if (result.IsSuccessStatusCode)
                {
                    var users = await result.Content.ReadFromJsonAsync<List<User>>();
                    if (users != null)
                    {
                        return users;
                    }
                }
                else
                {
                    var error = await result.Content.ReadAsStringAsync();
                    if (error != null && !string.IsNullOrEmpty(error))
                        AppServices.Error = error;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            var login = new Login
            {
                UserName = loginModel.UserName,
                Password = loginModel.Password,
            };

            HttpResponseMessage result = await httpClient.PostAsJsonAsync("Account/Login", login);
            if (result.IsSuccessStatusCode)
            {
                var tokenModel = await result.Content.ReadFromJsonAsync<TokenModel>();
                if (tokenModel != null)
                {
                    var customState = (AppAuthenticationStateProvider)stateProvider;
                    if (await customState.CreateAuthenticationStateAsync(tokenModel))
                    {
                        return true;
                    }
                }
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return false;
        }

        public async Task<bool> RegisterAsync(RegisterModel register)
        {
            if (register == null || register.Password == null || register.Email == null || register.Password.Length < 6)
                return false;

            AppServices.Error = null;
            var registerModel = new Register
            {
                Email = register.Email,
                UserName = register.UserName,
                Password = register.Password,
            };

            var httpClient = clientService.GetClient();
            HttpResponseMessage result = await httpClient
                .PostAsJsonAsync($"Account/Register", registerModel);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return false;
        }
    }
}
