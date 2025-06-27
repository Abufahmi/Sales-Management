using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Sales.Client.Repositories;
using Sales.Library.Contracts;
using System.Globalization;

namespace Sales.Client.Services;

public static class WebAppHost
{
    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
        var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
        CultureInfo culture;
        if (result != null)
            culture = new CultureInfo(result);
        else
            culture = new CultureInfo("en-US");

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public static IServiceCollection ConfigureRepositoryServices(this IServiceCollection services)
    {
        services.AddTransient<CustomHttpHandler>();
        services.AddTransient<LocalStorageService>();
        services.AddTransient<ClientService>();
        services.AddTransient<AuthenticationStateProvider, AppAuthenticationStateProvider>();
        services.AddTransient<IAccountService, AccountRepository>();
        services.AddTransient<ISiteRepository, SiteRepository>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IFileService, FileService>();
        return services;
    }
}
