using CartService.Interfaces.Repositories;
using CartService.Interfaces.Services;
using CartService.Models.Entities;
using System;
using System.Threading.Tasks;

namespace CartService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public ProductService(IProductRepository productRepository,
            ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task AddActiveProductAsync(Guid productIdentifier)
        {
            var product = await _productRepository.GetActiveProductAsync(productIdentifier);

            if (product == null)
            {
                var newProduct = new ActiveProduct { ProductIdentifier = productIdentifier };

                await _productRepository.SaveActiveProductAsync(newProduct);
            }
        }

        public async Task RemoveActiveProductAsync(Guid productIdentifier)
        {
            var product = await _productRepository.GetActiveProductAsync(productIdentifier);

            if (product != null)
            {
                await _productRepository.RemoveActiveProductAsync(product);

                await _cartRepository.DeleteCartsAsync(productIdentifier);
            }
        }
    }
}
