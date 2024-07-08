using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Sales.Client.Helpers;
using Sales.Client.Models.SettingModels;
using Sales.Client.Services;
using Sales.Library;
using System.Net.Http.Json;

namespace Sales.Client.Bases.SettingBases
{
    public class UserListBase : ComponentBase
    {
        [Inject] public ClientService ClientService { get; set; } = default!;
        public List<UserModel>? UserModels { get; set; }
        List<UserModel>? AllUsers { get; set; }
        public bool IsBusy { get; set; }
        public PaginationState Pagination { get; set; } = new();
        public int ItemPerPage { get; set; } = 3;

        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            var users = await GetUsersAsync();
            if (users != null)
            {
                UserModels = ModelConverter.GetUserModels(users);
                AllUsers = UserModels;
                Pagination.ItemsPerPage = ItemPerPage;
            }
            IsBusy = false;
            await base.OnInitializedAsync();
        }

        private async Task<List<User>?> GetUsersAsync()
        {
            AppServices.Error = null;
            var httpClient = ClientService.GetClient();
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
            return null;
        }

        protected void OnUserChanged(ChangeEventArgs args)
        {
            if (args?.Value == null)
            {
                UserModels = AllUsers;
            }
            else
            {
                var txt = args.Value as string;
                if (txt != null && AllUsers != null)
                {
                    txt = txt.Trim().ToLower();
                    var models = AllUsers
                        .Where(x => x.UserName!.ToLower().Contains(txt) ||
                        x.Email!.ToLower().Contains(txt) ||
                        x.PhoneNumber != null && x.PhoneNumber!.ToLower().Contains(txt))
                        .ToList();

                    if (models != null)
                    {
                        UserModels = models;
                    }
                    else
                    {
                        UserModels = AllUsers;
                    }
                }
            }
        }
    }
}
