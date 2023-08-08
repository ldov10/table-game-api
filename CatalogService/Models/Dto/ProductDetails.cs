using System;

namespace CatalogService.Models.Dto
{
    public class ProductDetails
    {
        public Guid Identifier { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public double Rating { get; set; }

        public string Brand { get; set; }

        public string Category { get; set; }
    }
}
