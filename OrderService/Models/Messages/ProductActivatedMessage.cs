using System;
using OrderService.Models.Enums;

namespace OrderService.Models.Messages
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
