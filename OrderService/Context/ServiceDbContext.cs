using Microsoft.EntityFrameworkCore;
using OrderService.Models.Entities;

namespace OrderService.Context
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ActiveProduct> ActiveProducts { get; set; }

        public DbSet<OrderHistory> OrderHistories { get; set; }

        public DbSet<OrderProductMapping> OrderProductMappings { get; set; }

        public DbSet<Address> Addresses { get; set; }
    }
}
