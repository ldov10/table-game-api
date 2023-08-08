using System;

namespace CatalogService.Models.Dto
{
    public class ProductInfo
    {
        public Guid Identifier { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ShortDescription { get; set; }
    }
}
