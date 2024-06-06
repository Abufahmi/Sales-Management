using Microsoft.EntityFrameworkCore;
using Sales.Context.Data;
using Sales.Library.Models;
using Sales.Library;
using static Sales.Context.Helpers.Responses;
using Sales.Context.Helpers;

namespace Sales.Context.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDb db;
        private readonly IDbService dbService;

        public AccountService(ApplicationDb db, IDbService dbService)
        {
            this.db = db;
            this.dbService = dbService;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<LoginResponse> LoginAsync(Login login)
        {
            await dbService.CreateAdminAsync();
            if (login == null || login.Password == null || login.UserName == null)
            {
                return new LoginResponse(false, "Model is empty");
            }

            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName);
            if (user == null || user.Password == null)
            {
                user = await db.Users.FirstOrDefaultAsync(x => x.Email == login.UserName);
                if (user == null)
                    return new LoginResponse(false, "There is no such account");
            }

            if (user.EmailConfirmed == false)
            {
                return new LoginResponse(false, "Registration account not confired yet");
            }
            if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password) == false)
            {
                return new LoginResponse(false, "User name or password is incorrect");
            }

            if (await db.Roles.AnyAsync() == false)
            {
                bool result = await dbService.CreateRolesAsync();
                if (!result)
                {
                    return new LoginResponse(false, "Internal server error");
                }
            }

            if (await db.UserRoles.AnyAsync(x => x.UserId == user.Id) == false)
            {
                await dbService.CreateUserRoleIfNotExistsAsync(user);
            }

            string? roleName = await dbService.GetRoleNameByUserIdAsync(user.Id!);
            if (roleName == null)
            {
                return new LoginResponse(false, "Could not set user authurazation");
            }

            string token = dbService.GenerateToken(user, roleName);
            string refreshToken = dbService.GenerateRefreshToken();

            bool loged = await dbService.CreateLoginAsync(user.Id!, refreshToken);
            if (!loged)
            {
                return new LoginResponse(false, "Could not set user login, Please try later");
            }
            return new LoginResponse(true, "Ok", token, refreshToken);
        }

        public async Task<TokenModel?> RefreshTokenAsync(string refreshToken)
        {
            LibraryService.Error = null;
            if (refreshToken == null || refreshToken == string.Empty)
            {
                LibraryService.Error = "Model is empty";
                return null;
            }

            var userLogin = await db.UserLogins
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (userLogin == null)
            {
                LibraryService.Error = "Refresh token is invalid";
                return null;
            }

            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userLogin.UserId);
            if (user == null)
            {
                LibraryService.Error = "Refresh token could not generated. User not found";
                return null;
            }

            string? roleName = await dbService.GetRoleNameByUserIdAsync(user.Id!);
            if (roleName == null)
            {
                LibraryService.Error = "Could not fetch user authurazation";
                return null;
            }

            try
            {
                string token = dbService.GenerateToken(user, roleName);
                refreshToken = dbService.GenerateRefreshToken();

                bool loged = await dbService.CreateLoginAsync(user.Id!, refreshToken);
                if (!loged)
                {
                    LibraryService.Error = "Could not set user login, Please try later";
                    return null;
                }

                return new TokenModel { Token = token, RefreshToken = refreshToken };
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return null;
            }
        }

        public async Task<DefaultResponse> RegisterAsync(Register register)
        {
            await dbService.CreateAdminAsync();
            if (register == null || register.UserName == null || register.Email == null || register.Password == null)
            {
                return new DefaultResponse(false, "Model is empty");
            }

            if (await db.Users.AnyAsync(x => x.Email == register.Email))
            {
                return new DefaultResponse(false, "Email address is already token");
            }
            if (await db.Users.AnyAsync(x => x.UserName == register.UserName))
            {
                return new DefaultResponse(false, "User name is already token");
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = register.Email,
                UserName = register.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                CreatedDate = DateTime.Now,
                PhoneConfirmed = false,
                EmailConfirmed = false,
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();
            return new DefaultResponse(true, "Account Created.");
        }
    }
}
