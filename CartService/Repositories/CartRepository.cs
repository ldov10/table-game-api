using CartService.Context;
using CartService.Interfaces.Repositories;
using CartService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ServiceDbContext _context;

        public CartRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cart>> GetUserCartsAsync(Guid identifier)
        {
            return await _context.Carts.Where(x => x.UserIdentifier == identifier).ToListAsync();
        }

        public async Task<Cart> GetCartAsync(Guid userIdentifier, Guid productIdentifier)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(x => x.UserIdentifier == userIdentifier && x.ProductIdentifier == productIdentifier);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();
        }

        public async Task SaveCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartsAsync(Guid productIdentifier)
        {
            var q = _context.Carts.Where(x => x.ProductIdentifier == productIdentifier);

            _context.Carts.RemoveRange(q);

            await _context.SaveChangesAsync();
        }
    }
}
