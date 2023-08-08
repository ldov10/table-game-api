using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;
using CatalogService.Models.Pagination;

namespace CatalogService.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(string title);

        Task<Product> GetProductAsync(Guid identifier);

        Task<List<Product>> GetProductsAsync(List<Guid> identifiers);

        Task SaveProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task<(List<Product> products, int notPagedCount)> GetProductPageAsync(PaginationParameters pagination,
            ProductsSearchOptions productsSearchOptions = null, bool isActive = false);

        Task DeleteProductAsync(Product product);

        Task<ActiveOrderProduct> GetOrderProductAsync(Guid productIdentifier);

        Task SaveOrderProductsAsync(List<ActiveOrderProduct> activeOrderProducts);

        Task RemoveOrderProductsAsync(Guid orderIdentifier);

        Task<Product> GetProductByBrandIdAsync(long brandId);

        Task<Product> GetProductByCategoryIdAsync(long categoryId);

        Task SaveBookmarkAsync(Bookmark bookmark);

        Task<Bookmark> GetBookmarkAsync(Guid userIdentifier, long productId);

        Task<Bookmark> GetBookmarkAsync(Guid identifier);

        Task DeleteBookmarkAsync(Bookmark bookmark);

        Task<List<Bookmark>> GetUserBookmarksAsync(Guid userIdentifier);
    }
}
