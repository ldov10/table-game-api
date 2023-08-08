using OrderService.Models.Enums;
using System;
using System.Collections.Generic;

namespace OrderService.Models.Messages
{
    public class OrderCreatedMessage : BaseMessage
    {
        public OrderCreatedMessage()
        {
            MessageType = MessageTypes.OrderCreated;
        }

        public Guid OrderIdentifier { get; set; }

        public List<Guid> ProductIdentifiers { get; set; }
    }
}
