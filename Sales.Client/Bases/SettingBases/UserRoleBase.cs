using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Sales.Client.Helpers;
using Sales.Client.Models.SettingModels;
using Sales.Client.Services;
using Sales.Library;
using System.Net.Http.Json;

namespace Sales.Client.Bases.SettingBases
{
    public class UserRoleBase : ComponentBase
    {
        [Inject] ClientService ClientService { get; set; } = default!;
        [Inject] NavigationManager Nav { get; set; } = default!;
        [Inject] IStringLocalizer<App> Localizer { get; set; } = default!;
        [Parameter] public string? Id { get; set; }
        public UserRoleModel UserRoleModel { get; set; } = new();
        public IEnumerable<User> Users { get; set; } = [];
        public IEnumerable<Role> Roles { get; set; } = [];
        IEnumerable<UserRole> UserRoles { get; set; } = [];
        public bool IsBusy { get; set; }
        public string? Error { get; set; }
        public string? Message { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var users = await GetUsersAsync();
            if (users != null)
            {
                Users = users;
            }

            Roles = await GetRolesAsync();
            UserRoles = await GetUserRolesAsync();
            await base.OnInitializedAsync();
        }

        private async Task<List<UserRole>> GetUserRolesAsync()
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient.GetAsync("Settings/GetUserRoles");
            if (result.IsSuccessStatusCode)
            {
                var userRoles = await result.Content.ReadFromJsonAsync<List<UserRole>>();
                if (userRoles != null)
                {
                    return userRoles;
                }
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return new();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Id != null)
            {
                var userRole = await GetUserRoleByIdAsync(Id);
                if (userRole == null)
                {
                    Nav.NavigateTo("/", true);
                }
                else
                {
                    UserRoleModel = ModelConverter.GetUserRoleModel(userRole);
                }
            }

            await base.OnParametersSetAsync();
        }

        private async Task<UserRole> GetUserRoleByIdAsync(string id)
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient.GetAsync($"Settings/GetUserRoleById/{id}");
            if (result.IsSuccessStatusCode)
            {
                var userRole = await result.Content.ReadFromJsonAsync<UserRole>();
                if (userRole != null)
                {
                    return userRole;
                }
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return new();
        }

        private async Task<List<Role>> GetRolesAsync()
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient.GetAsync("Settings/GetRoles");
            if (result.IsSuccessStatusCode)
            {
                var roles = await result.Content.ReadFromJsonAsync<List<Role>>();
                if (roles != null)
                {
                    return roles;
                }
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    AppServices.Error = error;
            }
            return new List<Role>();
        }

        private async Task<List<User>?> GetUsersAsync()
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient.GetAsync("Settings/GetUsers");
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
            return null;
        }

        protected void HideMessage()
        {
            Error = null;
            Message = null;
        }

        protected async Task OnValidSubmit()
        {
            Error = null;
            Message = null;
            if (IsUserRoleExists())
            {
                var userName = Users
                    .Where(x => x.Id == UserRoleModel.UserId)
                    .Select(x => x.UserName)
                    .FirstOrDefault();
                var roleName = Roles
                   .Where(x => x.Id == UserRoleModel.RoleId)
                   .Select(x => x.RoleName)
                   .FirstOrDefault();
                Error = string.Format(Localizer[nameof(Strings.UserRoleExists)].Value,
                    userName, roleName);
                return;
            }

            IsBusy = true;
            var userRole = new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = UserRoleModel.UserId,
                RoleId = UserRoleModel.RoleId,
            };

            if (Id == null)
            {
                bool result = await CreateUserRoleAsync(userRole);
                if (result)
                {
                    Message = Localizer[nameof(Strings.CreateSuccessMessage)].Value;
                    UserRoleModel = new();
                }
            }
            else
            {
                userRole.Id = UserRoleModel.Id;
                bool result = await UpdateUserRoleAsync(userRole);
                if (result)
                {
                    Nav.NavigateTo("UserRoles");
                }
            }
            IsBusy = false;
        }

        private async Task<bool> UpdateUserRoleAsync(UserRole userRole)
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient
                .PutAsJsonAsync("Settings/UpdateUserRole", userRole);
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

        private async Task<bool> CreateUserRoleAsync(UserRole userRole)
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient
                .PostAsJsonAsync("Settings/CreateUserRole", userRole);
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

        bool IsUserRoleExists()
        {
            return UserRoles.Any(x => x.UserId == UserRoleModel.UserId && x.RoleId == UserRoleModel.RoleId);
        }
    }
}
