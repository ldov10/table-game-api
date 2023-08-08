using CartService.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CartService.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetUserCartsAsync(Guid identifier);

        Task<Cart> GetCartAsync(Guid userIdentifier, Guid productIdentifier);

        Task UpdateCartAsync(Cart cart);

        Task SaveCartAsync(Cart cart);

        Task DeleteCartAsync(Cart cart);

        Task DeleteCartsAsync(Guid productIdentifier);
    }
}
