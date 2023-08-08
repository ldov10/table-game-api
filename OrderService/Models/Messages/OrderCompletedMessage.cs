using System;
using OrderService.Models.Enums;

namespace OrderService.Models.Messages
{
    public class OrderCompletedMessage : BaseMessage
    {
        public OrderCompletedMessage()
        {
            MessageType = MessageTypes.OrderCompleted;
        }

        public Guid OrderIdentifier { get; set; }
    }
}
