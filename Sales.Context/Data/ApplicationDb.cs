using Microsoft.EntityFrameworkCore;
using Sales.Library.Entities;

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
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<MainSetting> MainSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
