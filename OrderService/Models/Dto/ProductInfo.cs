using System;

namespace OrderService.Models.Dto
{
    public class ProductInfo
    {
        public Guid ProductIdentifier { get; set; }

        public int Quantity { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ShortDescription { get; set; }
    }
}
