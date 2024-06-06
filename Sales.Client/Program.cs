using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sales.Client;
using Sales.Client.Helpers;
using Sales.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Authintication
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Add BlazoredLocalStorage
builder.Services.AddBlazoredLocalStorage();

// In your Program.cs or Startup.cs
builder.Services.AddHttpClient("ApiClient",
    client => client.BaseAddress = new Uri(AppServices.BaseAddress))
    .AddHttpMessageHandler<CustomHttpHandler>();


// Add localization
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Configure services
builder.Services.ConfigureRepositoryServices();


var app = builder.Build();

// Configure localization
await app.SetDefaultCulture();

await app.RunAsync();

