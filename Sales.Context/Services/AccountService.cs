using Microsoft.EntityFrameworkCore;
using Sales.Context.Data;
using Sales.Library.Models;
using Sales.Library;
using static Sales.Context.Helpers.Responses;
using Sales.Context.Helpers;
using Sales.Context.Contracts;

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

        public async Task<LoginResponse> ForgetPasswordAsync(string email)
        {
            if (email == null || string.IsNullOrWhiteSpace(email))
            {
                return new LoginResponse(false, "Email address required");
            }

            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return new LoginResponse(false, "Email address required");
            }

            string verifyCode = LibraryService.GeneratePassword(6).ToUpper();
            while (db.Verifications.Any(x => x.VerificationCode == verifyCode))
            {
                verifyCode = LibraryService.GeneratePassword(6).ToUpper();
            }

            string token = LibraryService.GeneratePassword(60).ToUpper();

            var verification = new Verification
            {
                VerificationCode = verifyCode,
                CreatedDate = DateTime.Now,
                Token = token,
                UserId = user.Id,
            };

            await db.AddAsync(verification);
            await db.SaveChangesAsync();
            return new LoginResponse(true, "OK", token, verifyCode);
        }

        public async Task<MainSetting?> GetMainSettingAsync()
        {
            return await db.MainSettings.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<DefaultResponse> IsTokenExistsAsync(string token)
        {
            LibraryService.Error = null;
            if (token == null || token.Length == 0)
                return new DefaultResponse(false, "Token is required");

            var verification = await db.Verifications
                .FirstOrDefaultAsync(x => x.Token == token);
            if (verification == null)
            {
                return new DefaultResponse(false, "Token not found");
            }

            var minutes = await db.MainSettings
                .Select(x => x.TokenExpireMinutes).FirstOrDefaultAsync();
            if (minutes == 0)
                minutes = 5;

            if (verification.CreatedDate.AddMinutes(minutes) < DateTime.Now)
            {
                db.Remove(verification);
                await db.SaveChangesAsync();
                return new DefaultResponse(false, "Token is expired");
            }
            return new DefaultResponse(true);
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

        public async Task<bool> ResetAccountAsync(ResetAccount resetAccount)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == resetAccount.UserId);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetAccount.Password);
            db.Entry(user).Property(x => x.Password).IsModified = true;
            await db.SaveChangesAsync();

            var verifications = await db.Verifications
                .Where(x => x.UserId == user.Id).ToListAsync();

            if (verifications != null)
            {
                db.RemoveRange(verifications);
                await db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<string?> VerifyCodeAsync(string verificationCode)
        {
            LibraryService.Error = null;
            if (verificationCode == null || verificationCode.Length == 0)
                return null;

            var verification = await db.Verifications
                .FirstOrDefaultAsync(x => x.VerificationCode == verificationCode);
            if (verification == null)
            {
                return null;
            }
            return verification.UserId;
        }
    }
}
