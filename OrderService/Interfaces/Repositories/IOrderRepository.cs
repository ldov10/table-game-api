using OrderService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task SaveOrderAsync(Order order);

        Task SaveOrderProductMappingsAsync(List<OrderProductMapping> mappings);

        Task<Order> GetOrderAsync(Guid identifier);

        Task UpdateOrderAsync(Order order);

        Task SaveOrderHistoryAsync(OrderHistory orderHistory);

        Task<List<OrderHistory>> GetOrderHistoryAsync(long orderId);

        Task<List<Order>> GetOrdersAsync(Guid userIdentifier);

        Task<List<OrderProductMapping>> GetOrderMappingsAsync(Guid identifier);
    }
}
