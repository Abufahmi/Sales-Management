
using Sales.Client.Services;

namespace Sales.Client.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly ClientService clientService;
        private readonly IAccountRepository accountRepository;

        public SiteRepository(ClientService clientService, IAccountRepository accountRepository)
        {
            this.clientService = clientService;
            this.accountRepository = accountRepository;
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
    }
}
