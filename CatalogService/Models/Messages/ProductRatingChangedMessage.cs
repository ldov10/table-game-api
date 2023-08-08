using System;
using CatalogService.Models.Enums;

namespace CatalogService.Models.Messages
{
    public class ProductRatingChangedMessage : BaseMessage
    {
        public ProductRatingChangedMessage()
        {
            MessageType = MessageTypes.ProductRatingChanged;
        }

        public Guid ProductIdentifier { get; set; }

        public double Rating { get; set; }
    }
}
