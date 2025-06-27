using Microsoft.EntityFrameworkCore;
using Sales.Context.Contracts;
using Sales.Context.Data;
using Sales.Context.Helpers;
using Sales.Library.Entities;

namespace Sales.Context.Services
{
    public class MainSettingService : IMainSettingService
    {
        private readonly ApplicationDb db;

        public MainSettingService(ApplicationDb db)
        {
            this.db = db;
        }

        public async Task<MainSetting?> CreateMainSettingAsync(MainSetting mainSetting)
        {
            if (mainSetting == null) return null;
            try
            {
                db.Add(mainSetting);
                await db.SaveChangesAsync();
                return mainSetting;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return null;
            }
        }

        public async Task<MainSetting?> GetMainSettingAsync()
        {
            return await db.MainSettings.FirstOrDefaultAsync();
        }

        public async Task<MainSetting?> GetMainSettingByIdAsync(int id)
        {
            return await db.MainSettings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MainSetting?> UpdateMainSettingAsync(MainSetting mainSetting)
        {
            if (mainSetting == null) return null;
            try
            {
                var entity = await GetMainSettingByIdAsync(mainSetting.Id);
                if (entity == null) return null;

                entity.TokenExpireMinutes = mainSetting.TokenExpireMinutes;
                entity.ItemPerPage = mainSetting.ItemPerPage;

                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return null;
            }
        }
    }
}
