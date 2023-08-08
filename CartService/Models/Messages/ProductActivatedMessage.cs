using System;
using CartService.Models.Enums;

namespace CartService.Models.Messages
{
    public class ProductActivatedMessage : BaseMessage
    {
        public ProductActivatedMessage()
        {
            MessageType = MessageTypes.ProductRatingChanged;
        }

        public Guid ProductIdentifier { get; set; }
    }
}
