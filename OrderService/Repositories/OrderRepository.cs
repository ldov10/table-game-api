using Microsoft.EntityFrameworkCore;
using OrderService.Context;
using OrderService.Interfaces.Repositories;
using OrderService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ServiceDbContext _context;

        public OrderRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task SaveOrderAsync(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }

        public async Task SaveOrderProductMappingsAsync(List<OrderProductMapping> mappings)
        {
            await _context.OrderProductMappings.AddRangeAsync(mappings);

            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderAsync(Guid identifier)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();
        }

        public async Task SaveOrderHistoryAsync(OrderHistory orderHistory)
        {
            _context.OrderHistories.Add(orderHistory);

            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderHistory>> GetOrderHistoryAsync(long orderId)
        {
            return await _context.OrderHistories.Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersAsync(Guid userIdentifier)
        {
            return await _context.Orders.Where(x => x.UserIdentifier == userIdentifier).ToListAsync();
        }

        public async Task<List<OrderProductMapping>> GetOrderMappingsAsync(Guid identifier)
        {
            return await _context.OrderProductMappings.Where(x => x.OrderIdentifier == identifier).ToListAsync();
        }
    }
}
