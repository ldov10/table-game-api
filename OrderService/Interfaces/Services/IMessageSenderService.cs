using OrderService.Models.Messages;

namespace OrderService.Interfaces.Services
{
    public interface IMessageSenderService
    {
        void SendMessage(BaseMessage message, string queueName);
    }
}
