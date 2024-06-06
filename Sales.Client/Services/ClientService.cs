using Sales.Client.Helpers;
using Sales.Client.Models;
using Sales.Library.Models;

namespace Sales.Client.Services
{
    public class ClientService
    {
        private readonly LocalStorageService localStorageService;
        private readonly IHttpClientFactory httpClientFactory;
        private const string HeaderKey = "Authorazation";

        public ClientService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
        {
            this.httpClientFactory = httpClientFactory;
            this.localStorageService = localStorageService;
        }

        public async Task<HttpClient> GetAuthorizeClient()
        {
            var client = httpClientFactory.CreateClient("ApiClient");
            client.BaseAddress = new Uri(AppServices.BaseAddress);
            var stringToken = await localStorageService.GetToken();
            if (stringToken == null || string.IsNullOrEmpty(stringToken))
            {
                client.DefaultRequestHeaders.Remove(HeaderKey);
                return client;
            }

            var deserializeToken = Serialization.DeSerializeJsonString<TokenModel>(stringToken);
            if (deserializeToken == null)
            {
                client.DefaultRequestHeaders.Remove(HeaderKey);
                return client;
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                .AuthenticationHeaderValue("Bearer", deserializeToken.Token);
            return client;
        }

        public HttpClient GetClient()
        {
            var client = httpClientFactory.CreateClient("ApiClient");
            client.BaseAddress = new Uri(AppServices.BaseAddress);
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}