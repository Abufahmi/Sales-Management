using Microsoft.EntityFrameworkCore;
using Sales.Context.Contracts;
using Sales.Context.Data;
using Sales.Context.Helpers;
using Sales.Library;

namespace Sales.Context.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly ApplicationDb db;

        public UserRoleService(ApplicationDb db)
        {
            this.db = db;
        }

        public async Task<bool> CreateUserRoleAsync(UserRole userRole)
        {
            if (userRole == null) return false;
            try
            {
                if (db.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId))
                {
                    LibraryService.Error = "User Already in role";
                    return false;
                }

                await db.AddAsync(userRole);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return false;
            }
        }

        public async Task<bool> DeleteUserRoleByIdAsync(string id)
        {
            LibraryService.Error = null;
            try
            {
                var userRole = await db.UserRoles.FirstOrDefaultAsync(x => x.Id == id);
                if (userRole == null) return false;
                db.Remove(userRole);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return false;
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await db.Roles.ToListAsync();
        }

        public async Task<UserRole?> GetUserRoleByIdAsync(string id)
        {
            return await db.UserRoles
             .Include(x => x.User)
             .Include(x => x.Role)
             .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesAsync()
        {
            return await db.UserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .ToListAsync();
        }

        public async Task<bool> UpdateUserRoleAsync(UserRole userRole)
        {
            if (userRole == null) throw new ArgumentNullException(nameof(userRole));
            try
            {
                if (db.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId &&
                    x.Id != userRole.Id))
                {
                    LibraryService.Error = "User Already in role";
                    return false;
                }

                LibraryService.Error = null;
                var entity = await GetUserRoleByIdAsync(userRole.Id!);
                if (entity == null) return false;

                entity.UserId = userRole.UserId;
                entity.RoleId = userRole.RoleId;

                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return false;
            }
        }
    }
}
