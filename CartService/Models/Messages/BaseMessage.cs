using CartService.Models.Enums;

namespace CartService.Models.Messages
{
    public class BaseMessage
    {
        public MessageTypes MessageType { get; set; }
    }
}
