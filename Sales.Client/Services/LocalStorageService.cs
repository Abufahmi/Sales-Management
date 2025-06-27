using Blazored.LocalStorage;

namespace Sales.Client.Services;

public class LocalStorageService
{
    private readonly ILocalStorageService localStorageService;

    public LocalStorageService(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public const string StorageKey = "authitication-token";

    public async Task<string?> GetToken()
    {
        return await localStorageService.GetItemAsStringAsync(StorageKey);
    }

    public async Task RemoveToken()
    {
        await localStorageService.RemoveItemAsync(StorageKey);
    }

    public async Task SetToken(string item)
    {
        await localStorageService.SetItemAsStringAsync(StorageKey, item);
    }
}
