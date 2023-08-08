using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using OrderService.Exceptions;
using OrderService.Interfaces.Repositories;
using OrderService.Interfaces.Services;
using OrderService.Models.Dto;
using OrderService.Models.Entities;
using OrderService.Models.Enums;
using OrderService.Models.Messages;
using OrderService.Options;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageSenderService _messageSenderService;
        private readonly RabbitMqQueuesOptions _rabbitMqQueuesOptions;

        public OrderService(IMapper mapper,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IOptions<RabbitMqQueuesOptions> rabbitMqQueuesOptions,
            IMessageSenderService messageSenderService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _rabbitMqQueuesOptions = rabbitMqQueuesOptions.Value;
            _messageSenderService = messageSenderService;
        }

        public async Task PostOrderAsync(Guid userIdentifier, OrderCreationDto orderCreationDto)
        {
            var productIds = orderCreationDto.Products.Select(x => x.ProductIdentifier).Distinct().ToList();

            if (productIds.Count != orderCreationDto.Products.Count)
            {
                throw new InternalException("Invalid products.");
            }

            var activeProducts = await _productRepository.GetActiveProductsAsync(productIds);

            if (productIds.Count != activeProducts.Count)
            {
                throw new InternalException("Invalid products.");
            }

            orderCreationDto.Address.Phone = orderCreationDto.Address.Phone.Trim();
            orderCreationDto.Address.City = orderCreationDto.Address.City.Trim();
            orderCreationDto.Address.AddressString = orderCreationDto.Address.AddressString.Trim();
            orderCreationDto.Address.Country = orderCreationDto.Address.Country.Trim();
            orderCreationDto.Address.Zip = orderCreationDto.Address.Zip.Trim();

            var address = _mapper.Map<Address>(orderCreationDto.Address);

            var order = new Order
            {
                Address = address,
                UserIdentifier = userIdentifier,
                Comments = orderCreationDto.Comments,
                OrderState = OrderStates.Created
            };

            var products = new List<OrderProductMapping>();

            foreach (var product in orderCreationDto.Products)
            {
                var activeProduct = activeProducts.FirstOrDefault(x => x.ProductIdentifier == product.ProductIdentifier);

                products.Add(new OrderProductMapping
                {
                    OrderIdentifier = order.Identifier,
                    ProductIdentifier = product.ProductIdentifier,
                    Quantity = product.Quantity,
                    Title = activeProduct.Title,
                    Price = activeProduct.Price,
                    ShortDescription = activeProduct.ShortDescription
                });
            }

            await _orderRepository.SaveOrderAsync(order);
            await _orderRepository.SaveOrderProductMappingsAsync(products);

            var message = new OrderCreatedMessage { OrderIdentifier = order.Identifier, ProductIdentifiers = productIds };
            _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CatalogService);
        }

        public async Task UpdateOrderStateAsync(Guid userIdentifier,
            Guid orderIdentifier,
            UserRoles userRole,
            OrderStates newState)
        {
            if (newState == OrderStates.Created)
            {
                throw new InternalException("Invalid order state.");
            }

            if ((int)userRole <= 0 || (int)userRole > Enum.GetNames(typeof(UserRoles)).Length)
            {
                throw new InternalException("Invalid user role.");
            }

            var order = await _orderRepository.GetOrderAsync(orderIdentifier);

            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            if (newState == OrderStates.Confirmed)
            {
                if (order.OrderState != OrderStates.Created)
                {
                    throw new InternalException("Invalid order state.");
                }

                if (userRole != UserRoles.Manager && userRole != UserRoles.Admin)
                {
                    throw new InternalException("Invalid user role.");
                }

                var orderHistory = new OrderHistory
                {
                    Order = order,
                    UserRole = userRole,
                    OldState = order.OrderState,
                    NewState = newState,
                    UserIdentifier = userIdentifier
                };

                await _orderRepository.SaveOrderHistoryAsync(orderHistory);

                order.OrderState = newState;
                await _orderRepository.UpdateOrderAsync(order);

                return;
            }

            if (newState == OrderStates.Cancelled)
            {
                if (order.OrderState == OrderStates.Completed || order.OrderState == OrderStates.Cancelled)
                {
                    throw new InternalException("Invalid order state.");
                }

                if (userRole != UserRoles.Manager && userRole != UserRoles.Admin)
                {
                    if (userIdentifier != order.UserIdentifier)
                    {
                        throw new InternalException("Invalid user.");
                    }
                }

                var orderHistory = new OrderHistory
                {
                    Order = order,
                    UserRole = userRole,
                    OldState = order.OrderState,
                    NewState = newState,
                    UserIdentifier = userIdentifier
                };

                await _orderRepository.SaveOrderHistoryAsync(orderHistory);

                order.OrderState = newState;
                await _orderRepository.UpdateOrderAsync(order);

                var message = new OrderCompletedMessage { OrderIdentifier = order.Identifier };
                _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CatalogService);

                return;
            }

            if (newState == OrderStates.Completed)
            {
                if (order.OrderState != OrderStates.Confirmed)
                {
                    throw new InternalException("Invalid order state.");
                }

                if (userRole != UserRoles.Manager && userRole != UserRoles.Admin)
                {
                    if (userIdentifier != order.UserIdentifier)
                    {
                        throw new InternalException("Invalid user.");
                    }
                }

                var orderHistory = await _orderRepository.GetOrderHistoryAsync(order.Id);

                if (!orderHistory.Where(x => x.NewState == OrderStates.Completed).Any())
                {
                    var newOrderHistory = new OrderHistory
                    {
                        Order = order,
                        UserRole = userRole,
                        OldState = order.OrderState,
                        NewState = newState,
                        UserIdentifier = userIdentifier
                    };

                    await _orderRepository.SaveOrderHistoryAsync(newOrderHistory);

                    return;
                }

                if (orderHistory.Where(x => x.UserIdentifier == userIdentifier && x.NewState == OrderStates.Completed).Any())
                {
                    if (userRole != UserRoles.Manager && userRole != UserRoles.Admin)
                    {
                        throw new InternalException("Invalid user role.");
                    }

                    var newOrderHistory = new OrderHistory
                    {
                        Order = order,
                        UserRole = userRole,
                        OldState = order.OrderState,
                        NewState = newState,
                        UserIdentifier = userIdentifier
                    };

                    await _orderRepository.SaveOrderHistoryAsync(newOrderHistory);

                    order.OrderState = newState;
                    await _orderRepository.UpdateOrderAsync(order);

                    var message = new OrderCompletedMessage { OrderIdentifier = order.Identifier };
                    _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CatalogService);

                    return;
                }
                else
                {
                    if (userIdentifier != order.UserIdentifier)
                    {
                        throw new InternalException("Invalid user.");
                    }

                    var newOrderHistory = new OrderHistory
                    {
                        Order = order,
                        UserRole = userRole,
                        OldState = order.OrderState,
                        NewState = newState,
                        UserIdentifier = userIdentifier
                    };

                    await _orderRepository.SaveOrderHistoryAsync(newOrderHistory);

                    order.OrderState = newState;
                    await _orderRepository.UpdateOrderAsync(order);

                    var message = new OrderCompletedMessage { OrderIdentifier = order.Identifier };
                    _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CatalogService);

                    return;
                }
            }
        }

        public async Task<List<OrderInfo>> GetOrdersInfoAsync(Guid userIdentifier)
        {
            var orders = await _orderRepository.GetOrdersAsync(userIdentifier);

            if (orders == null || !orders.Any())
            {
                return new List<OrderInfo>();
            }

            var ordersInfo = new List<OrderInfo>();

            foreach (var order in orders)
            {
                var orderMappings = await _orderRepository.GetOrderMappingsAsync(order.Identifier);

                var orderHistory = await _orderRepository.GetOrderHistoryAsync(order.Id);

                ordersInfo.Add(new OrderInfo
                {
                    OrderIdentifier = order.Identifier,
                    OrderState = order.OrderState,
                    Products = _mapper.Map<List<ProductInfo>>(orderMappings),
                    ManagerApproval = orderHistory.Where(x => x.UserRole != UserRoles.User &&
                        x.NewState == OrderStates.Completed).Any(),
                    UserApproval = orderHistory.Where(x => x.UserIdentifier == userIdentifier &&
                        x.NewState == OrderStates.Completed).Any()
                });
            }

            return ordersInfo;
        }
    }
}
