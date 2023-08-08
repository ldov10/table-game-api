﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Enums;
using CatalogService.Models.Messages;
using CatalogService.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CatalogService.Services
{
    public class MessageReceiverService : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IProductService _productService;
        private readonly RabbitMqOptions _rabbitMqOptions;

        public MessageReceiverService(IProductService productService,
            IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _rabbitMqOptions = rabbitMqOptions.Value;
            _productService = productService;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitMqOptions.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(content);

                try
                {
                    switch (baseMessage.MessageType)
                    {
                        case MessageTypes.ProductRatingChanged:

                            var productRatingChangedMessage = JsonConvert.DeserializeObject<ProductRatingChangedMessage>(content);
                            await _productService.UpdateProductRatingAsync(productRatingChangedMessage.ProductIdentifier,
                                productRatingChangedMessage.Rating);
                            break;

                        case MessageTypes.OrderCreated:

                            var orderCreatedMessage = JsonConvert.DeserializeObject<OrderCreatedMessage>(content);
                            await _productService.AddOrderProductsAsync(orderCreatedMessage);
                            break;

                        case MessageTypes.OrderCompleted:

                            var orderCompletedMessage = JsonConvert.DeserializeObject<OrderCompletedMessage>(content);
                            await _productService.RemoveOrderProductsAsync(orderCompletedMessage.OrderIdentifier);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception) { }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_rabbitMqOptions.QueueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
