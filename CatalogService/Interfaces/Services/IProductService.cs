using CatalogService.Models.Dto;
using System.Threading.Tasks;
using CatalogService.Models.Pagination;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CatalogService.Models.Messages;

namespace CatalogService.Interfaces.Services
{
    public interface IProductService
    {
        Task PostProductAsync(ProductCreationDto product);

        Task<PagedList<ProductsPageItem>> GetProductPageAsync(PaginationParameters pagination,
            ProductsSearchOptions productsSearchOptions, bool isActive = true);

        Task<List<ProductInfo>> GetProductsAsync(List<Guid> productsIds);

        Task<ProductDetails> GetProductDetailsAsync(Guid identifier);

        Task UpdateProductAsync(Guid identifier, ProductUpdateDto product);

        Task UpdateProductPriceAsync(Guid identifier, decimal price);

        Task UpdateProductActiveStateAsync(Guid identifier, bool isActive);

        Task DeleteProductAsync(Guid identifier);

        Task PostProductImageAsync(Guid productIdentifier, IFormFile file);

        Task<byte[]> GetProductImageAsync(Guid identifier);

        Task<byte[]> GetProductFirstImageAsync(Guid identifier);

        Task<List<Guid>> GetProductImagesIdsAsync(Guid identifier);

        Task UpdateProductRatingAsync(Guid identifier, double rating);

        Task AddOrderProductsAsync(OrderCreatedMessage message);

        Task RemoveOrderProductsAsync(Guid orderIdentifier);

        Task PostBookmarkAsync(Guid userIdentifier, Guid productIdentifier);

        Task DeleteBookmarkAsync(Guid userIdentifier, Guid bookmarkIdentifier);

        Task<List<BookmarkInfo>> GetUserBookmarksAsync(Guid userIdentifier);
    }
}
