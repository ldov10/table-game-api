using System;
using CatalogService.Models.Enums;

namespace CatalogService.Models.Messages
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
