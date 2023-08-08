using Microsoft.EntityFrameworkCore;
using UserService.Models.Entities;

namespace UserService.Context
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                Email = "email@email.com",
                FirstName = "admin",
                LastName = "admin",
                Username = "admin",
                Password = "admin",
                Role = Models.Enums.Roles.Admin
            });
        }
    }
}
