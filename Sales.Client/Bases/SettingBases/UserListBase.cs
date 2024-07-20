using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using Sales.Client.Helpers;
using Sales.Client.Models.SettingModels;
using Sales.Client.Repositories;
using Sales.Client.Services;
using Sales.Library;
using System.Net.Http.Json;

namespace Sales.Client.Bases.SettingBases
{
    public class UserListBase : ComponentBase
    {
        [Inject] public ClientService ClientService { get; set; } = default!;
        [Inject] IJSRuntime Js { get; set; } = default!;
        [Inject] IAccountRepository Repository { get; set; } = default!;
        public List<UserModel>? UserModels { get; set; }
        List<UserModel>? AllUsers { get; set; }
        public bool IsBusy { get; set; }
        public PaginationState Pagination { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            var users = await GetUsersAsync();
            if (users != null)
            {
                UserModels = ModelConverter.GetUserModels(users);
                AllUsers = UserModels;
                if (AppServices.ItemPerPage == 0)
                {
                    var main = await Repository.GetMainSettingAsync();
                    if (main != null && main.ItemPerPage != 0)
                    {
                        Pagination.ItemsPerPage = main.ItemPerPage;
                    }
                    else
                    {
                        Pagination.ItemsPerPage = 10;
                    }
                }
                else
                {
                    Pagination.ItemsPerPage = AppServices.ItemPerPage;
                }
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

        protected async Task DeleteConfirmAsync(string? id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return;
            }

            var result = await Js.InvokeAsync<bool>("openMessageComponent");
            if (result == true)
            {
                foreach (var item in UserModels!)
                {
                    if (item.Id == id)
                    {
                        item.IsSelected = true;
                    }
                    else
                    {
                        item.IsSelected = false;
                    }
                }
            }
        }

        protected async Task ApproveResultAsync(bool args)
        {
            if (args == true)
            {
                var userModel = UserModels!.FirstOrDefault(x => x.IsSelected == true);
                if (userModel != null)
                {
                    bool result = await DeleteUserByIdAsync(userModel.Id!);
                    if (result)
                    {
                        UserModels?.Remove(userModel);
                    }
                }
            }

            foreach (var item in UserModels!)
            {
                item.IsSelected = false;
            }
        }

        private async Task<bool> DeleteUserByIdAsync(string id)
        {
            AppServices.Error = null;
            var httpClient = await ClientService.GetAuthorizeClientAsync();
            HttpResponseMessage result = await httpClient.GetAsync($"Settings/DeleteUserById/{id}");
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
