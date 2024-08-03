using Microsoft.EntityFrameworkCore;
using Sales.Context.Data;
using Sales.Context.Helpers;
using Sales.Library;

namespace Sales.Context.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDb db;

        public UserService(ApplicationDb db)
        {
            this.db = db;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            if (user == null) return false;
            LibraryService.Error = null;
            try
            {
                if (db.Users.Any(x => x.UserName == user.UserName))
                {
                    LibraryService.Error = "User name already exists";
                    return false;
                }
                if (db.Users.Any(x => x.Email == user.Email))
                {
                    LibraryService.Error = "Email address already exists";
                    return false;
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await db.AddAsync(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return false;
            }
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return false;
            db.Remove(user);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>?> GetUsersAsync()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            LibraryService.Error = null;
            if (user == null) throw new ArgumentNullException("user");
            try
            {
                if (db.Users.Any(x => x.UserName == user.UserName && x.Id != user.Id))
                {
                    LibraryService.Error = "User name already exists";
                    return false;
                }
                if (db.Users.Any(x => x.Email == user.Email && x.Id != user.Id))
                {
                    LibraryService.Error = "Email address already exists";
                    return false;
                }

                var entity = await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                if (entity == null) return false;

                entity.UserName = user.UserName;
                entity.Email = user.Email;
                entity.EmailConfirmed = user.EmailConfirmed;
                entity.PhoneConfirmed = user.PhoneConfirmed;
                entity.Image = user.Image;
                entity.PhoneNumber = user.PhoneNumber;

                if (entity.Password != user.Password)
                {
                    entity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

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
