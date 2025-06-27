using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Sales.Client.Helpers;
using Sales.Client.Models.SettingModels;
using Sales.Client.Services;
using Sales.Library.Entities;
using System.Net.Http.Json;
using Translate = Sales.Client.Resources.App;

namespace Sales.Client.Bases.SettingBases;

public class UserBase : ComponentBase
{
    [Inject] ClientService ClientService { get; set; } = default!;
    [Inject] IStringLocalizer<App> Localizer { get; set; } = default!;
    [Inject] NavigationManager Nav { get; set; } = default!;
    [Parameter] public string? Id { get; set; }
    public UserModel UserModel { get; set; } = new UserModel();
    List<User>? Users { get; set; }
    public bool IsBusy { get; set; }
    public string? Error { get; set; }
    public string? Message { get; set; }
    public string? EmailError { get; set; }
    public string? UserNameError { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;
        if (Id != null)
        {
            var user = await GetUserAsync(Id);
            if (user != null)
            {
                UserModel = ModelConverter.GetUserModel(user);
            }
            else
            {
                Nav.NavigateTo("/", true);
            }
        }

        Users = await GetUsersAsync();
        IsBusy = false;
        await base.OnParametersSetAsync();
    }

    private async Task<User?> GetUserAsync(string id)
    {
        AppServices.Error = null;
        var httpClient = await ClientService.GetAuthorizeClientAsync();
        HttpResponseMessage result = await httpClient.GetAsync($"Settings/GetUserById/{id}");
        if (result.IsSuccessStatusCode)
        {
            var user = await result.Content.ReadFromJsonAsync<User>();
            return user;
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return null;
    }

    private async Task<List<User>?> GetUsersAsync()
    {
        AppServices.Error = null;
        var httpClient = await ClientService.GetAuthorizeClientAsync();
        HttpResponseMessage result = await httpClient.GetAsync("Settings/GetUserList");
        if (result.IsSuccessStatusCode)
        {
            var users = await result.Content.ReadFromJsonAsync<List<User>>();
            return users;
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
        if (UserNameError != null || EmailError != null)
        {
            return;
        }

        IsBusy = true;
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.Now,
            Email = UserModel.Email,
            EmailConfirmed = UserModel.EmailConfirmed,
            Image = null,
            Password = UserModel.Password,
            PhoneConfirmed = UserModel.PhoneConfirmed,
            PhoneNumber = UserModel.PhoneNumber,
            UserName = UserModel.UserName,
        };

        if (Id != null)
        {
            user.Id = Id;
            var result = await UpdateUserAsync(user);
            if (result)
            {
                Nav.NavigateTo("/Users");
            }
            else
            {
                if (AppServices.Error != null)
                    Error = AppServices.Error;
            }
        }
        else
        {
            if (await CreateUserAsync(user))
            {
                Message = Localizer[nameof(Translate.CreateSuccessMessage)]?.Value;
            }
            else
            {
                if (AppServices.Error != null)
                    Error = AppServices.Error;
            }
        }

        IsBusy = false;
    }

    private async Task<bool> UpdateUserAsync(User user)
    {
        AppServices.Error = null;
        var httpClient = await ClientService.GetAuthorizeClientAsync();
        HttpResponseMessage result = await httpClient
            .PutAsJsonAsync("Settings/UpdateUser", user);
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

    private async Task<bool> CreateUserAsync(User user)
    {
        AppServices.Error = null;
        var httpClient = await ClientService.GetAuthorizeClientAsync();
        HttpResponseMessage result = await httpClient
            .PostAsJsonAsync("Settings/CreateUser", user);
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

    protected void OnUserNameChange(ChangeEventArgs args)
    {
        UserNameError = null;
        if (args?.Value == null) return;
        var txt = args.Value as string;
        if (string.IsNullOrWhiteSpace(txt) || Users == null)
            return;

        if (Users.Any(x => x.UserName!.ToLower() == txt.ToLower()))
            UserNameError = Localizer[nameof(Translate.UserNameExists)]?.Value;
    }

    protected void OnEmailChange(ChangeEventArgs args)
    {
        EmailError = null;
        if (args?.Value == null) return;
        var txt = args.Value as string;
        if (string.IsNullOrWhiteSpace(txt) || Users == null)
            return;

        if (Users.Any(x => x.Email!.ToLower() == txt.ToLower()))
            EmailError = Localizer[nameof(Translate.EmailAddressExists)]?.Value;
    }
}
