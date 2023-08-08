using System;
using CatalogService.Models.Enums;

namespace CatalogService.Models.Messages
{
    public class ProductActivatedMessage : BaseMessage
    {
        public ProductActivatedMessage()
        {
            MessageType = MessageTypes.ProductActivated;
        }

        public Guid ProductIdentifier { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ShortDescription { get; set; }
    }
}
