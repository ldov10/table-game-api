using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Models.Dto;
using OrderService.Models.Enums;

namespace OrderService.Interfaces.Services
{
    public interface IOrderService
    {
        Task PostOrderAsync(Guid userIdentifier, OrderCreationDto orderCreationDto);

        Task UpdateOrderStateAsync(Guid userIdentifier,
            Guid orderIdentifier,
            UserRoles userRole,
            OrderStates newState);

        Task<List<OrderInfo>> GetOrdersInfoAsync(Guid userIdentifier);
    }
}
