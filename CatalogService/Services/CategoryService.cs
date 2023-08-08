using System;
using AutoMapper;
using CatalogService.Interfaces.Repositories;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Dto;
using CatalogService.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Exceptions;

namespace CatalogService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task PostCategoryAsync(CategoryCreationDto category)
        {
            var existingCategory = await _categoryRepository.GetCategoryAsync(category.Title);

            if (existingCategory != null)
            {
                throw new InternalException($"Category with Title {category.Title} already exist");
            }

            category.Title = category.Title.Trim();

            var newCategory = _mapper.Map<Category>(category);

            await _categoryRepository.SaveCategoryAsync(newCategory);
        }

        public async Task UpdateCategoryAsync(Guid identifier, CategoryUpdateDto category)
        {
            var existingCategory = await _categoryRepository.GetCategoryAsync(identifier);

            if (existingCategory == null)
            {
                throw new NotFoundException("Category not found.");
            }

            existingCategory.Title = category.Title.Trim();

            await _categoryRepository.UpdateCategoryAsync(existingCategory);
        }

        public async Task<List<CategoryDetails>> GetCategoryListAsync()
        {
            var categories = await _categoryRepository.GetCategoryListAsync();

            var categoryList = _mapper.Map<List<CategoryDetails>>(categories);

            return categoryList;
        }

        public async Task RemoveCategoryAsync(Guid identifier)
        {
            var existingCategory = await _categoryRepository.GetCategoryAsync(identifier);

            if (existingCategory == null)
            {
                throw new NotFoundException("Category not found.");
            }

            var categoryProduct = await _productRepository.GetProductByCategoryIdAsync(existingCategory.Id);

            if (categoryProduct != null)
            {
                throw new InternalException("Category can not be deleted.");
            }

            await _categoryRepository.RemoveCategoryAsync(existingCategory);
        }
    }
}
