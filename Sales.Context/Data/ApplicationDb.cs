using Microsoft.EntityFrameworkCore;
using Sales.Library;

namespace Sales.Context.Data
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
    }
}
