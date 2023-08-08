using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System;
using Newtonsoft.Json;
using OrderService.Interfaces.Services;
using OrderService.Models.Messages;
using OrderService.Options;

namespace OrderService.Services
{
    public class MessageSenderService : IMessageSenderService
    {
        private readonly RabbitMqOptions _rabbitMqOptions;
        private IConnection _connection;

        public MessageSenderService(IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _rabbitMqOptions = rabbitMqOptions.Value;

            CreateConnection();
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqOptions.HostName,
                    UserName = _rabbitMqOptions.UserName,
                    Password = _rabbitMqOptions.Password,
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception) { }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }
    }
}
