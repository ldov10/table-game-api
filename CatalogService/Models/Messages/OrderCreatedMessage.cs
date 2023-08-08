using CatalogService.Models.Enums;
using System;
using System.Collections.Generic;

namespace CatalogService.Models.Messages
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
