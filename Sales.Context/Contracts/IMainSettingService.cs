using Sales.Library;

namespace Sales.Context.Contracts
{
    public interface IMainSettingService
    {
        Task<MainSetting?> CreateMainSettingAsync(MainSetting mainSetting);
        Task<MainSetting?> GetMainSettingAsync();
        Task<MainSetting?> GetMainSettingByIdAsync(int id);
        Task<MainSetting?> UpdateMainSettingAsync(MainSetting mainSetting);
    }
}
