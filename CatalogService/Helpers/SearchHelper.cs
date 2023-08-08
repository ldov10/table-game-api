using System.Linq;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;

namespace CatalogService.Helpers
{
    public static class SearchHelper
    {
        public static IQueryable<Product> BuildSearchQuery(IQueryable<Product> query,
            ProductsSearchOptions productsSearchOptions)
        {
            if (productsSearchOptions == null)
            {
                return query;
            }

            var newQuery = query;

            if (!string.IsNullOrEmpty(productsSearchOptions.Text))
            {
                productsSearchOptions.Text = productsSearchOptions.Text.Trim();

                newQuery = newQuery.Where(x =>
                    x.Title.Contains(productsSearchOptions.Text) ||
                    x.ShortDescription.Contains(productsSearchOptions.Text));
            }

            if (productsSearchOptions.BrandsList != null && productsSearchOptions.BrandsList.Any())
            {
                newQuery = newQuery
                    .Where(x => productsSearchOptions.BrandsList.Contains(x.Brand.Identifier));
            }

            if (productsSearchOptions.CategoriesList != null && productsSearchOptions.CategoriesList.Any())
            {
                newQuery = newQuery
                    .Where(x => productsSearchOptions.CategoriesList.Contains(x.Category.Identifier));
            }

            if (productsSearchOptions.MaxAge.HasValue)
            {
                newQuery = newQuery
                    .Where(x => x.Age <= productsSearchOptions.MaxAge);
            }

            if (productsSearchOptions.MinAge.HasValue)
            {
                newQuery = newQuery
                    .Where(x => x.Age >= productsSearchOptions.MinAge);
            }

            if (productsSearchOptions.Players.HasValue)
            {
                newQuery = newQuery
                    .Where(x => x.MinPlayers <= productsSearchOptions.Players &&
                                x.MaxPlayers >= productsSearchOptions.Players);
            }

            if (productsSearchOptions.MaxPrice.HasValue)
            {
                newQuery = newQuery
                    .Where(x => x.Price <= productsSearchOptions.MaxPrice);
            }

            if (productsSearchOptions.MinRating.HasValue)
            {
                newQuery = newQuery
                    .Where(x => x.Rating >= productsSearchOptions.MinRating);
            }

            return newQuery;
        }
    }
}
