using System;
using UserService.Models.Enums;

namespace UserService.Models.Messages
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
