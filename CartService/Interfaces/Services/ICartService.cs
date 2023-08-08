using CartService.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CartService.Interfaces.Services
{
    public interface ICartService
    {
        Task<List<CartItem>> GetUserCartItemsAsync(Guid userIdentifier);

        Task AddProductToCartAsync(Guid userIdentifier, CartItemDto cartItemDto);

        Task DeleteCartItemAsync(Guid userIdentifier, Guid productIdentifier);

        Task SetCartItemQuantityAsync(Guid userIdentifier, Guid productIdentifier, int quantity);
    }
}
