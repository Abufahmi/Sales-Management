using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sales.Context.Data;
using Sales.Context.Helpers;
using Sales.Context.Models;
using Sales.Library;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sales.Context.Services
{
    public class DbService: IDbService
    {
        private readonly ApplicationDb db;
        private readonly IOptions<JwtSection> options;

        public DbService(ApplicationDb db, IOptions<JwtSection> options)
        {
            this.db = db;
            this.options = options;
        }

        public async Task<bool> CreateLoginAsync(string userId, string refreshToken)
        {
            if (userId == null || refreshToken == null)
            {
                return false;
            }

            var userLogin = new UserLogin
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RefreshToken = refreshToken,
                LoginDate = DateTime.UtcNow,
            };
            db.Add(userLogin);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateRolesAsync()
        {
            if (db.Roles.Any())
            {
                return true;
            }

            try
            {
                var roles = new List<Role>
            {
                new Role{Id = Guid.NewGuid().ToString(), RoleName = LibraryService.Admin},
                new Role{Id = Guid.NewGuid().ToString(), RoleName = LibraryService.Visor},
                new Role{Id = Guid.NewGuid().ToString(), RoleName = LibraryService.User},
            };
                await db.AddRangeAsync(roles);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LibraryService.Error = ex.Message;
                return false;
            }
        }

        public async Task<bool> CreateUserRoleIfNotExistsAsync(User user)
        {
            var role = await db.Roles.FirstOrDefaultAsync(x => x.RoleName == LibraryService.User);
            if (role == null)
                return false;

            var userRole = new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                RoleId = role.Id,
            };
            await db.AddAsync(userRole);
            await db.SaveChangesAsync();
            return true;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateToken(User user, string roleName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, roleName),
            };

            var token = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> GetRoleNameByUserIdAsync(string userId)
        {
            var userRole = await db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole == null)
                return null;

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
            if (role == null)
                return null;

            return role.RoleName;
        }
    }
}
