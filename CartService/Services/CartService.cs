using AutoMapper;
using CartService.Exceptions;
using CartService.Interfaces.Repositories;
using CartService.Interfaces.Services;
using CartService.Models.Dto;
using CartService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Services
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(IMapper mapper,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<List<CartItem>> GetUserCartItemsAsync(Guid userIdentifier)
        {
            var carts = await _cartRepository.GetUserCartsAsync(userIdentifier);

            if (carts == null)
            {
                throw new InternalException("Internal exception.");
            }

            return _mapper.Map<List<CartItem>>(carts);
        }

        public async Task AddProductToCartAsync(Guid userIdentifier, CartItemDto cartItemDto)
        {
            var activeProduct = await _productRepository.GetActiveProductAsync(cartItemDto.ProductIdentifier);

            if (activeProduct == null)
            {
                throw new InternalException("Invalid product.");
            }

            var cart = await _cartRepository.GetCartAsync(userIdentifier, cartItemDto.ProductIdentifier);

            if(cart != null)
            {
                cart.Quantity++;

                await _cartRepository.UpdateCartAsync(cart);

                return;
            }

            var newCartItem = _mapper.Map<Cart>(cartItemDto);

            newCartItem.UserIdentifier = userIdentifier;

            await _cartRepository.SaveCartAsync(newCartItem);
        }

        public async Task DeleteCartItemAsync(Guid userIdentifier, Guid productIdentifier)
        {
            var cart = await _cartRepository.GetCartAsync(userIdentifier, productIdentifier);

            if (cart == null)
            {
                throw new NotFoundException("Cart item not found.");
            }

            await _cartRepository.DeleteCartAsync(cart);
        }

        public async Task SetCartItemQuantityAsync(Guid userIdentifier, Guid productIdentifier, int quantity)
        {
            if (quantity <= 0)
            {
                throw new InternalException("Invalid Quantity.");
            }

            var cart = await _cartRepository.GetCartAsync(userIdentifier, productIdentifier);

            if (cart == null)
            {
                throw new NotFoundException("Cart item not found.");
            }

            cart.Quantity = quantity;

            await _cartRepository.UpdateCartAsync(cart);
        }
    }
}
