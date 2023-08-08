using System;
using System.Collections.Generic;
using CatalogService.Context;
using System.Threading.Tasks;
using CatalogService.Helpers;
using CatalogService.Interfaces.Repositories;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;
using CatalogService.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CatalogService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ServiceDbContext _context;

        public ProductRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductAsync(string title)
        {
            title = title.Trim().ToUpper();

            return await _context.Products
                .FirstOrDefaultAsync(x => string.Equals(x.Title, title));
        }

        public async Task<Product> GetProductAsync(Guid identifier)
        {
            return await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task<List<Product>> GetProductsAsync(List<Guid> identifiers)
        {
            return await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Where(x => identifiers.Contains(x.Identifier))
                .ToListAsync();
        }

        public async Task SaveProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);

            await _context.SaveChangesAsync();
        }

        public async Task<(List<Product> products, int notPagedCount)> GetProductPageAsync(PaginationParameters pagination,
            ProductsSearchOptions productsSearchOptions = null, bool isActive = false)
        {
            var baseQuery = _context.Products.AsQueryable();

            if (isActive)
            {
                baseQuery = baseQuery
                   .Include(x => x.Brand)
                   .Include(x => x.Category)
                   .Where(x => x.IsActive);
            }
            else
            {
                baseQuery = baseQuery
                    .Include(x => x.Brand)
                    .Include(x => x.Category)
                    .Where(x => !x.IsActive);
            }

            if (productsSearchOptions == null)
            {
                return (await PaginationHelper.GetPagedListAsync(baseQuery, pagination), await baseQuery.CountAsync());
            }

            var searchQuery = SearchHelper.BuildSearchQuery(baseQuery, productsSearchOptions);

            return (await PaginationHelper.GetPagedListAsync(searchQuery, pagination), await searchQuery.CountAsync());
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task<ActiveOrderProduct> GetOrderProductAsync(Guid productIdentifier)
        {
            return await _context.ActiveOrdersProducts.FirstOrDefaultAsync(x => x.ProductIdentifier == productIdentifier);
        }

        public async Task SaveOrderProductsAsync(List<ActiveOrderProduct> activeOrderProducts)
        {
            await _context.ActiveOrdersProducts.AddRangeAsync(activeOrderProducts);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrderProductsAsync(Guid orderIdentifier)
        {
            var products = _context.ActiveOrdersProducts.Where(x => x.OrderIdentifier == orderIdentifier);

            _context.ActiveOrdersProducts.RemoveRange(products);

            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductByBrandIdAsync(long brandId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.BrandId == brandId);
        }

        public async Task<Product> GetProductByCategoryIdAsync(long categoryId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }

        public async Task SaveBookmarkAsync(Bookmark bookmark)
        {
            await _context.Bookmarks.AddAsync(bookmark);

            await _context.SaveChangesAsync();
        }

        public async Task<Bookmark> GetBookmarkAsync(Guid userIdentifier, long productId)
        {
            return await _context.Bookmarks
                .FirstOrDefaultAsync(x => x.UserIdentifier == userIdentifier && x.ProductId == productId);
        }

        public async Task<Bookmark> GetBookmarkAsync(Guid identifier)
        {
            return await _context.Bookmarks.FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task DeleteBookmarkAsync(Bookmark bookmark)
        {
            _context.Bookmarks.Remove(bookmark);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Bookmark>> GetUserBookmarksAsync(Guid userIdentifier)
        {
            return await _context.Bookmarks
                .Include(x => x.Product)
                .Where(x => x.UserIdentifier == userIdentifier && x.Product.IsActive)
                .ToListAsync();
        }
    }
}
