using CartService.Context;
using CartService.Interfaces.Repositories;
using CartService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ServiceDbContext _context;

        public ProductRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task SaveActiveProductAsync(ActiveProduct product)
        {
            _context.ActiveProducts.Add(product);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveActiveProductAsync(ActiveProduct product)
        {
            _context.ActiveProducts.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task<ActiveProduct> GetActiveProductAsync(Guid identifier)
        {
            return await _context.ActiveProducts.FirstOrDefaultAsync(x => x.ProductIdentifier == identifier);
        }
    }
}
