using CartService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartService.Context
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<ActiveProduct> ActiveProducts { get; set; }

        public DbSet<Cart> Carts { get; set; }
    }
}
