using OrderService.Context;
using OrderService.Interfaces.Repositories;
using OrderService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Repositories
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

        public async Task<List<ActiveProduct>> GetActiveProductsAsync(List<Guid> products)
        {
            return await _context.ActiveProducts
                .Where(x => products.Contains(x.ProductIdentifier))
                .ToListAsync();
        }
    }
}
