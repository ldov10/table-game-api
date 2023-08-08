using CatalogService.Models.Messages;

namespace CatalogService.Interfaces.Services
{
    public interface IMessageSenderService
    {
        void SendMessage(BaseMessage message, string queueName);
    }
}
