
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Sales.Client.Services;
using Sales.Library.Contracts;
using System.Security.Claims;

namespace Sales.Client.Repositories;

public class SiteRepository(
    IAccountService accountRepository,
    AuthenticationStateProvider authenticationState
    ) : ISiteRepository
{
    public async Task<int> GetItemPerPageAsync()
    {
        var main = await accountRepository.GetMainSettingAsync();
        if (main == null || main.ItemPerPage == 0)
        {
            return 10;
        }
        return main.ItemPerPage;
    }

    public async Task<string?> GetUserIdentityAsync()
    {
        var stateProvider = authenticationState as AppAuthenticationStateProvider;
        if (stateProvider == null) return null;
        var state = await stateProvider.GetAuthenticationStateAsync();
        if (state == null) return null;
        var id = state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null) return null;
        return id;
    }
}
