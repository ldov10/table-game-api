using System;
using System.Threading.Tasks;
using CartService.Interfaces.Services;
using CartService.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("getUserCartItems/{identifier}")]
        public async Task<IActionResult> GetUserCartItems(Guid identifier)
        {
            return Ok(await _cartService.GetUserCartItemsAsync(identifier));
        }

        [HttpDelete("removeProductFromCart/user/{userIdentifier}/product/{productIdentifier}")]
        public async Task<IActionResult> RemoveProductFromCart(Guid userIdentifier, Guid productIdentifier)
        {
            await _cartService.DeleteCartItemAsync(userIdentifier, productIdentifier);
            return Ok();
        }

        [HttpPut("setCartQuantity/user/{userIdentifier}/product/{productIdentifier}/quantity/{quantity}")]
        public async Task<IActionResult> SetCartQuantity(Guid userIdentifier, Guid productIdentifier, int quantity)
        {
            await _cartService.SetCartItemQuantityAsync(userIdentifier, productIdentifier, quantity);
            return Ok();
        }

        [HttpPost("addProductToCart/{userIdentifier}")]
        public async Task<IActionResult> AddProductToCart(Guid userIdentifier, [FromBody] CartItemDto cartItemDto)
        {
            await _cartService.AddProductToCartAsync(userIdentifier, cartItemDto);
            return Ok();
        }
    }
}
