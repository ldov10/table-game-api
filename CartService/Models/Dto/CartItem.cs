using System;

namespace CartService.Models.Dto
{
    public class CartItem
    {
        public Guid Identifier { get; set; }

        public Guid ProductIdentifier { get; set; }

        public int Quantity { get; set; }
    }
}
