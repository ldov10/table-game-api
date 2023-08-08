using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CatalogService.Exceptions;
using CatalogService.Interfaces.Repositories;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;
using CatalogService.Models.Messages;
using CatalogService.Models.Pagination;
using CatalogService.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CatalogService.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMessageSenderService _messageSenderService;
        private readonly RabbitMqQueuesOptions _rabbitMqQueuesOptions;

        private const long FileMaxSize = 5242880;

        public ProductService(IProductRepository productRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IImageRepository imageRepository,
            IMessageSenderService messageSenderService,
            IOptions<RabbitMqQueuesOptions> rabbitMqQueuesOptions)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
            _messageSenderService = messageSenderService;
            _rabbitMqQueuesOptions = rabbitMqQueuesOptions.Value;
        }

        public async Task PostProductAsync(ProductCreationDto product)
        {
            var existingProduct = await _productRepository.GetProductAsync(product.Title);

            if (existingProduct != null)
            {
                throw new InternalException($"Product with Title {product.Title} already exist");
            }

            var brand = await _brandRepository.GetBrandAsync(product.BrandIdentifier);

            if (brand == null)
            {
                throw new InternalException("Invalid brand.");
            }

            var category = await _categoryRepository.GetCategoryAsync(product.CategoryIdentifier);

            if (category == null)
            {
                throw new InternalException("Invalid category.");
            }

            product.Title = product.Title.Trim();
            product.ShortDescription = product.ShortDescription.Trim();
            product.Description = product.Description.Trim();

            var newProduct = _mapper.Map<Product>(product);

            newProduct.Brand = brand;
            newProduct.Category = category;

            await _productRepository.SaveProductAsync(newProduct);

            SendProductActivatedMessage(newProduct);
        }

        public async Task UpdateProductAsync(Guid identifier, ProductUpdateDto product)
        {
            var existingProduct = await _productRepository.GetProductAsync(identifier);

            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (!string.Equals(product.Title, existingProduct.Title))
            {
                var productWithSameTitle = await _productRepository.GetProductAsync(product.Title);

                if (productWithSameTitle != null)
                {
                    throw new InternalException($"Product with Title {product.Title} already exist");
                }
            }

            var brand = await _brandRepository.GetBrandAsync(product.BrandIdentifier);

            if (brand == null)
            {
                throw new InternalException("Invalid brand.");
            }

            var category = await _categoryRepository.GetCategoryAsync(product.CategoryIdentifier);

            if (category == null)
            {
                throw new InternalException("Invalid category.");
            }


            existingProduct.Title = product.Title.Trim();
            existingProduct.Description = product.Description.Trim();
            existingProduct.ShortDescription = product.ShortDescription.Trim();
            existingProduct.MinPlayers = product.MinPlayers;
            existingProduct.MaxPlayers = product.MaxPlayers;
            existingProduct.Age = product.Age;

            existingProduct.Brand = brand;
            existingProduct.Category = category;

            await _productRepository.UpdateProductAsync(existingProduct);
        }

        public async Task UpdateProductPriceAsync(Guid identifier, decimal price)
        {
            var existingProduct = await _productRepository.GetProductAsync(identifier);

            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found");
            }
            
            existingProduct.Price = price;

            await _productRepository.UpdateProductAsync(existingProduct);
        }

        public async Task UpdateProductActiveStateAsync(Guid identifier, bool isActive)
        {
            var existingProduct = await _productRepository.GetProductAsync(identifier);

            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found");
            }

            existingProduct.IsActive = isActive;

            await _productRepository.UpdateProductAsync(existingProduct);

            if (isActive)
            {
                SendProductActivatedMessage(existingProduct);
            }
            else
            {
                SendProductDeactivatedMessage(existingProduct.Identifier);
            }
        }

        public async Task DeleteProductAsync(Guid identifier)
        {
            var existingProduct = await _productRepository.GetProductAsync(identifier);

            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found");
            }

            var activeOrderProduct = await _productRepository.GetOrderProductAsync(identifier);

            if (activeOrderProduct != null)
            {
                throw new InternalException("Product can not be deleted.");
            }

            await _productRepository.DeleteProductAsync(existingProduct);

            SendProductDeactivatedMessage(existingProduct.Identifier);
        }

        public async Task<PagedList<ProductsPageItem>> GetProductPageAsync(PaginationParameters pagination,
            ProductsSearchOptions productsSearchOptions, bool isActive = true)
        {
            var products = await _productRepository.GetProductPageAsync(pagination, productsSearchOptions, true);

            var pagedProducts = _mapper.Map<List<ProductsPageItem>>(products.products);

            return new PagedList<ProductsPageItem>(pagedProducts, products.notPagedCount, pagination);
        }

        public async Task<List<ProductInfo>> GetProductsAsync(List<Guid> productsIds)
        {
            if (productsIds == null || !productsIds.Any())
            {
                return new List<ProductInfo>();
            }

            var products = await _productRepository.GetProductsAsync(productsIds);

            return _mapper.Map<List<ProductInfo>>(products);
        }

        public async Task<ProductDetails> GetProductDetailsAsync(Guid identifier)
        {
            var product = await _productRepository.GetProductAsync(identifier);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            var productDetails = _mapper.Map<ProductDetails>(product);

            return productDetails;
        }

        public async Task PostProductImageAsync(Guid productIdentifier, IFormFile file)
        {
            var product = await _productRepository.GetProductAsync(productIdentifier);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            if (file.Length > FileMaxSize)
            {
                throw new InternalException("File is more then 5mb.");
            }

            if (file.Length == 0)
            {
                throw new InternalException("File length is 0.");
            }

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            var image = new Image
            {
                Product = product,
                Data = memoryStream.ToArray()
            };

            await _imageRepository.SaveImageAsync(image);
        }

        public async Task<byte[]> GetProductImageAsync(Guid identifier)
        {

            var image = await _imageRepository.GetProductImageAsync(identifier);

            if (image == null)
            {
                throw new NotFoundException("Image not found.");
            }

            return image.Data;
        }

        public async Task<byte[]> GetProductFirstImageAsync(Guid identifier)
        {
            var product = await _productRepository.GetProductAsync(identifier);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            var image = await _imageRepository.GetProductFirstImageAsync(product.Id);

            if (image == null)
            {
                throw new NotFoundException("Image not found.");
            }

            return image.Data;
        }

        public async Task<List<Guid>> GetProductImagesIdsAsync(Guid identifier)
        {
            var product = await _productRepository.GetProductAsync(identifier);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            return await _imageRepository.GetProductImagesIdsAsync(product.Id);
        }

        public async Task UpdateProductRatingAsync(Guid identifier, double rating)
        {
            var product = await _productRepository.GetProductAsync(identifier);

            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }

            product.Rating = rating;

            await _productRepository.UpdateProductAsync(product);
        }

        public async Task AddOrderProductsAsync(OrderCreatedMessage message)
        {
            var orderProducts = new List<ActiveOrderProduct>();

            foreach (var product in message.ProductIdentifiers)
            {
                orderProducts.Add(new ActiveOrderProduct 
                { 
                    OrderIdentifier = message.OrderIdentifier, ProductIdentifier = product 
                });
            }

            await _productRepository.SaveOrderProductsAsync(orderProducts);
        }

        public async Task RemoveOrderProductsAsync(Guid orderIdentifier)
        {
            await _productRepository.RemoveOrderProductsAsync(orderIdentifier);
        }

        public async Task PostBookmarkAsync(Guid userIdentifier, Guid productIdentifier)
        {
            var product = await _productRepository.GetProductAsync(productIdentifier);

            if (product == null || !product.IsActive)
            {
                throw new NotFoundException("Product not found.");
            }

            var existingBookmark = await _productRepository.GetBookmarkAsync(userIdentifier, product.Id);

            if (existingBookmark != null)
            {
                throw new InternalException("Bookmark already exists.");
            }

            var bookmark = new Bookmark { UserIdentifier = userIdentifier, Product = product };

            await _productRepository.SaveBookmarkAsync(bookmark);
        }

        public async Task DeleteBookmarkAsync(Guid userIdentifier, Guid bookmarkIdentifier)
        {
            var bookmark = await _productRepository.GetBookmarkAsync(bookmarkIdentifier);

            if (bookmark == null)
            {
                throw new NotFoundException("Bookmark not found.");
            }

            await _productRepository.DeleteBookmarkAsync(bookmark);
        }

        public async Task<List<BookmarkInfo>> GetUserBookmarksAsync(Guid userIdentifier)
        {
            var userBookmarks = await _productRepository.GetUserBookmarksAsync(userIdentifier);

            return _mapper.Map<List<BookmarkInfo>>(userBookmarks);
        }

        private void SendProductActivatedMessage(Product product)
        {
            var message = _mapper.Map<ProductActivatedMessage>(product);

            _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CartService);
            _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.OrderService);
        }

        private void SendProductDeactivatedMessage(Guid productIdentifier)
        {
            var message = new ProductDeactivatedMessage
            {
                ProductIdentifier = productIdentifier
            };

            _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CartService);
            _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.OrderService);
        }
    }
}
