using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using Sales.Client.Helpers;
using Sales.Client.Models.SettingModels;
using Sales.Client.Repositories;
using Sales.Client.Services;
using Sales.Library.Entities;
using System.Net.Http.Json;

namespace Sales.Client.Bases.SettingBases;

public class UserRoleListBase : ComponentBase
{
    [Inject] public ClientService ClientService { get; set; } = default!;
    [Inject] IJSRuntime Js { get; set; } = default!;
    [Inject] ISiteRepository SiteRepository { get; set; } = default!;
    public List<UserRoleModel>? UserRoleModels { get; set; }
    List<UserRoleModel>? AllUserRole { get; set; }
    public bool IsBusy { get; set; }
    public PaginationState Pagination { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        var userRoles = await GetUserRolesAsync();
        if (userRoles != null)
        {
            UserRoleModels = ModelConverter.GetUserRoleModels(userRoles);
            AllUserRole = UserRoleModels;
            Pagination.ItemsPerPage = await SiteRepository.GetItemPerPageAsync();
        }
        IsBusy = false;
        await base.OnInitializedAsync();
    }

    private async Task<List<UserRole>?> GetUserRolesAsync()
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
        return null;
    }

    protected void OnSearchChanged(ChangeEventArgs args)
    {
        if (args?.Value == null)
        {
            UserRoleModels = AllUserRole;
        }
        else
        {
            var txt = args.Value as string;
            if (txt != null && AllUserRole != null)
            {
                txt = txt.Trim().ToLower();
                var models = AllUserRole
                    .Where(x => x.User!.UserName!.ToLower().Contains(txt) ||
                    x.Role!.RoleName!.ToLower().Contains(txt))
                    .ToList();

                if (models != null)
                {
                    UserRoleModels = models;
                }
                else
                {
                    UserRoleModels = AllUserRole;
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
            foreach (var item in UserRoleModels!)
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
            var userModel = UserRoleModels!.FirstOrDefault(x => x.IsSelected == true);
            if (userModel != null)
            {
                bool result = await DeleteUserRoleByIdAsync(userModel.Id!);
                if (result)
                {
                    UserRoleModels?.Remove(userModel);
                }
            }
        }

        foreach (var item in UserRoleModels!)
        {
            item.IsSelected = false;
        }
    }

    private async Task<bool> DeleteUserRoleByIdAsync(string id)
    {
        AppServices.Error = null;
        var httpClient = await ClientService.GetAuthorizeClientAsync();
        HttpResponseMessage result = await httpClient.DeleteAsync($"Settings/DeleteUserRoleById/{id}");
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
