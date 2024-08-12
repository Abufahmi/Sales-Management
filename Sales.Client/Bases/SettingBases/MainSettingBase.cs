using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Sales.Client.Helpers;
using Sales.Client.Models.SettingModels;
using Sales.Client.Services;
using Sales.Library;
using System.Net.Http.Json;

namespace Sales.Client.Bases.SettingBases
{
    public class MainSettingBase : ComponentBase
    {
        [Inject] ClientService ClientService { get; set; } = default!;
        [Inject] IStringLocalizer<App> Localizer { get; set; } = default!;
        [Inject] NavigationManager NavManager { get; set; } = default!;
        [Parameter] public int? Id { get; set; }
        public MainSettingModel MainSetting { get; set; } = new();
        public bool IsBusy { get; set; }
        public string? Error { get; set; }
        public string? Message { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id != null)
            {
                var mainSetting = await GetMainSettingByIdAsync(Id);
                if (mainSetting != null)
                {
                    MainSetting = ModelConverter.GetMainSettingModel(mainSetting);
                }
                else
                {
                    NavManager.NavigateTo("Dashboard", true);
                }
            }
            else
            {
                var mainSetting = await GetMainSettingAsync();
                if (mainSetting != null)
                {
                    NavManager.NavigateTo("Dashboard", true);
                }
            }

            await base.OnInitializedAsync();
        }

        private async Task<MainSetting?> GetMainSettingByIdAsync(int? id)
        {
            if (id == null || id == 0) return null;
            var client = await ClientService.GetAuthorizeClientAsync();
            var responseMessage = await client
                .GetAsync($"Settings/GetMainSettingById/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadFromJsonAsync<MainSetting>();
            }
            return null;
        }

        private async Task<MainSetting?> GetMainSettingAsync()
        {
            var client = await ClientService.GetAuthorizeClientAsync();
            var responseMessage = await client.GetAsync($"Settings/GetMainSetting");
            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadFromJsonAsync<MainSetting>();
            }
            return null;
        }

        public void DismissMessage()
        {
            Error = null;
            Message = null;
        }

        public async Task OnValidSubmit()
        {
            IsBusy = true;
            if (Id == null)
            {
                await CreateMainSettings();
            }
            else
            {
                await UpdateMainSettings();
            }
            IsBusy = false;
        }

        private async Task CreateMainSettings()
        {
            var mainSetting = new MainSetting
            {
                TokenExpireMinutes = MainSetting.TokenExpireMinutes,
                ItemPerPage = MainSetting.ItemPerPage,
            };

            var client = await ClientService.GetAuthorizeClientAsync();
            var responseMessage = await client
                .PostAsJsonAsync($"Settings/CreateMainSetting", mainSetting);
            if (responseMessage.IsSuccessStatusCode == false)
            {
                var error = await responseMessage.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    Error = error;
            }
            else
            {
                NavManager.NavigateTo("Dashboard", true);
            }
        }

        private async Task UpdateMainSettings()
        {
            IsBusy = true;
            var mainSetting = new MainSetting
            {
                Id = MainSetting.Id,
                TokenExpireMinutes = MainSetting.TokenExpireMinutes,
                ItemPerPage = MainSetting.ItemPerPage,
            };

            var client = await ClientService.GetAuthorizeClientAsync();
            var responseMessage = await client
                .PutAsJsonAsync($"Settings/UpdateMainSetting", mainSetting);
            if (responseMessage.IsSuccessStatusCode == false)
            {
                var error = await responseMessage.Content.ReadAsStringAsync();
                if (error != null && !string.IsNullOrEmpty(error))
                    Error = error;
            }
            else
            {
                Message = Localizer[nameof(Strings.UpdateSuccessMessage)]?.Value;
            }
            IsBusy = false;
        }
    }
}
