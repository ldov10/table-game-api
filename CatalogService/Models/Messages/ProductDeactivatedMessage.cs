using System;
using CatalogService.Models.Enums;

namespace CatalogService.Models.Messages
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
