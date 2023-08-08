using OrderService.Models.Enums;

namespace OrderService.Models.Messages
{
    public class BaseMessage
    {
        public MessageTypes MessageType { get; set; }
    }
}
