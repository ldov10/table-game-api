using AutoMapper;
using OrderService.Interfaces.Repositories;
using OrderService.Interfaces.Services;
using OrderService.Models.Entities;
using OrderService.Models.Messages;
using System;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddActiveProductAsync(ProductActivatedMessage message)
        {
            var product = await _productRepository.GetActiveProductAsync(message.ProductIdentifier);

            if (product == null)
            {
                var newProduct = _mapper.Map<ActiveProduct>(message);

                await _productRepository.SaveActiveProductAsync(newProduct);
            }
        }

        public async Task RemoveActiveProductAsync(Guid productIdentifier)
        {
            var product = await _productRepository.GetActiveProductAsync(productIdentifier);

            if (product != null)
            {
                await _productRepository.RemoveActiveProductAsync(product);
            }
        }
    }
}
