using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CatalogService.Exceptions;
using CatalogService.Interfaces.Repositories;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;

namespace CatalogService.Services
{
    public class BrandService : IBrandService
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;

        public BrandService(IBrandRepository brandRepository,
            IMapper mapper,
            IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task PostBrandAsync(BrandCreationDto brand)
        {
            var existingBrand = await _brandRepository.GetBrandAsync(brand.Title);

            if (existingBrand != null)
            {
                throw new InternalException($"Brand with Title {brand.Title} already exist");
            }

            brand.Title = brand.Title.Trim();

            var newBrand = _mapper.Map<Brand>(brand);

            await _brandRepository.SaveBrandAsync(newBrand);
        }

        public async Task UpdateBrandAsync(Guid identifier, BrandUpdateDto brand)
        {
            var existingBrand = await _brandRepository.GetBrandAsync(identifier);

            if (existingBrand == null)
            {
                throw new NotFoundException("Brand not found.");
            }

            existingBrand.Title = brand.Title.Trim();

            await _brandRepository.UpdateBrandAsync(existingBrand);
        }

        public async Task<List<BrandDetails>> GetBrandListAsync()
        {
            var brands = await _brandRepository.GetBrandListAsync();

            var brandList = _mapper.Map<List<BrandDetails>>(brands);

            return brandList;
        }

        public async Task RemoveBrandAsync(Guid identifier)
        {
            var existingBrand = await _brandRepository.GetBrandAsync(identifier);

            if (existingBrand == null)
            {
                throw new NotFoundException("Brand not found.");
            }

            var brandProduct = await _productRepository.GetProductByBrandIdAsync(existingBrand.Id);

            if (brandProduct != null)
            {
                throw new InternalException("Brand can not be deleted.");
            }

            await _brandRepository.RemoveBrandAsync(existingBrand);
        }
    }
}
