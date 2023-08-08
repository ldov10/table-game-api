using System;
using CartService.Models.Enums;

namespace CartService.Models.Messages
{
    public class ProductDeactivatedMessage : BaseMessage
    {
        public ProductDeactivatedMessage()
        {
            MessageType = MessageTypes.ProductRatingChanged;
        }

        public Guid ProductIdentifier { get; set; }
    }
}
