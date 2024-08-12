
using Microsoft.AspNetCore.Components.Authorization;
using Sales.Client.Services;
using System.Security.Claims;

namespace Sales.Client.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly ClientService clientService;
        private readonly IAccountRepository accountRepository;
        private readonly AuthenticationStateProvider authenticationState;

        public SiteRepository(ClientService clientService, IAccountRepository accountRepository,
            AuthenticationStateProvider authenticationState)
        {
            this.clientService = clientService;
            this.accountRepository = accountRepository;
            this.authenticationState = authenticationState;
        }

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
}
