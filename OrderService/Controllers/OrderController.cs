using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces.Services;
using OrderService.Models.Dto;
using OrderService.Models.Enums;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("postOrder/user/{userIdentifier}")]
        public async Task<IActionResult> PostOrder(Guid userIdentifier, [FromBody] OrderCreationDto orderCreationDto)
        {
            await _orderService.PostOrderAsync(userIdentifier, orderCreationDto);
            return Ok();
        }

        [HttpPut("updateOrderState/user/{userIdentifier}/order/{orderIdentifier}/newState/{newState}/userRole/{userRole}")]
        public async Task<IActionResult> UpdateOrderState(Guid userIdentifier, Guid orderIdentifier, string userRole, OrderStates newState)
        {
            Enum.TryParse(userRole, out UserRoles role);

            await _orderService.UpdateOrderStateAsync(userIdentifier, orderIdentifier, role, newState);
            return Ok();
        }

        [HttpGet("getOrders/user/{userIdentifier}")]
        public async Task<IActionResult> GetOrders(Guid userIdentifier)
        {
            return Ok(await _orderService.GetOrdersInfoAsync(userIdentifier));
        }
    }
}
