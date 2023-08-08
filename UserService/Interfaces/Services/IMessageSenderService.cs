using UserService.Models.Messages;

namespace UserService.Interfaces.Services
{
    public interface IMessageSenderService
    {
        void SendMessage(BaseMessage message, string queueName);
    }
}
