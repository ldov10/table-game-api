using System;
using System.Collections.Generic;

namespace CatalogService.Models.Dto
{
    public class ProductsSearchOptions
    {
        public string Text { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public int? Players { get; set; }

        public double? MinRating { get; set; }

        public List<Guid> BrandsList { get; set; }

        public List<Guid> CategoriesList { get; set; }
    }
}
