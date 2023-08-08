using System;
using OrderService.Models.Enums;

namespace OrderService.Models.Messages
{
    public class ProductDeactivatedMessage : BaseMessage
    {
        public ProductDeactivatedMessage()
        {
            MessageType = MessageTypes.ProductDeactivated;
        }

        public Guid ProductIdentifier { get; set; }
    }
}
