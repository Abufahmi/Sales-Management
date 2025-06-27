using Microsoft.AspNetCore.Components.Authorization;
using Sales.Client.Helpers;
using Sales.Client.Models;
using Sales.Client.Services;
using Sales.Library.Contracts;
using Sales.Library.Entities;
using Sales.Library.Models;
using System.Net.Http.Json;

namespace Sales.Client.Repositories
{
    public class AccountRepository : IAccountService
    {
        private readonly ClientService clientService;
        private readonly AuthenticationStateProvider stateProvider;

        public AccountRepository(ClientService clientService, AuthenticationStateProvider stateProvider)
        {
            this.clientService = clientService;
            this.stateProvider = stateProvider;
        }

        public async Task<string?> ForgetPasswordAsync(ForgetPassword forgetPassword)
        {
            if (forgetPassword?.EmailAddress == null || forgetPassword?.EmailModel?.Subject == null ||
                forgetPassword?.EmailModel?.Body == null)
                return null;

            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            var url = "Account/ForgetPassword";
            HttpResponseMessage result = await httpClient.PostAsJsonAsync(url, forgetPassword);
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();
                if (token != null)
                    return token;
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return null;
        }

        public async Task<MainSetting?> GetMainSettingAsync()
        {
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            var url = "Account/GetMainSetting";
            HttpResponseMessage result = await httpClient.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var main = await result.Content.ReadFromJsonAsync<MainSetting>();
                if (main != null)
                    return main;
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return null;
        }

        public async Task<TokenModel?> GetRefreshTokenAsync(RefreshTokenModel refreshToken)
        {
            if (refreshToken?.RefreshToken == null) return null;
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            var url = "Account/RefreshToken";
            HttpResponseMessage result = await httpClient.PostAsJsonAsync(url, refreshToken);
            if (result.IsSuccessStatusCode)
            {
                var tokenModel = await result.Content.ReadFromJsonAsync<TokenModel>();
                if (tokenModel != null)
                {
                    return tokenModel;
                }
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return null;
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

        public async Task<bool> IsTokenExistsAsync(string token)
        {
            if (token == null) return false;
            AppServices.Error = null;
            var httpClient = await clientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient
                .GetAsync($"Account/IsTokenExists/{token}");
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

        public async Task<bool> LoginAsync(Login login)
        {
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
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

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var customState = (AppAuthenticationStateProvider)stateProvider;
                await customState.DeleteAuthenticationStateAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppServices.Error = ex.Message;
            }

            return false;
        }

        public async Task<bool> RegisterAsync(Register register)
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

        public async Task<bool> ResetPasswordAsync(ResetAccount reset)
        {
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            HttpResponseMessage result = await httpClient
                .PostAsJsonAsync("Account/ResetAccount", reset);
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

        public async Task<string?> VerifyCodeAsync(string? verificationCode)
        {
            if (verificationCode == null) return null;
            AppServices.Error = null;
            var httpClient = clientService.GetClient();
            HttpResponseMessage result = await httpClient
                .GetAsync($"Account/VerifyCode/{verificationCode}");
            if (result.IsSuccessStatusCode)
            {
                var id = await result.Content.ReadAsStringAsync();
                if (id != null)
                {
                    return id;
                }
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return null;
        }
    }
}
