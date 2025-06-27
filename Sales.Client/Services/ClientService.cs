using Sales.Client.Helpers;
using Sales.Library.Models;

namespace Sales.Client.Services;

public class ClientService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
{
    private const string HeaderKey = "Authorazation";

    public async Task<HttpClient> GetAuthorizeClientAsync()
    {
        var client = httpClientFactory.CreateClient("ApiClient");
        client.BaseAddress = new Uri(AppServices.BaseApiAddress);
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
        client.BaseAddress = new Uri(AppServices.BaseApiAddress);
        client.DefaultRequestHeaders.Remove(HeaderKey);
        return client;
    }
}