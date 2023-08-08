using System.Collections.Generic;
using CatalogService.Models.Dto;
using System.Threading.Tasks;
using System;

namespace CatalogService.Interfaces.Services
{
    public interface ICategoryService
    {
        Task PostCategoryAsync(CategoryCreationDto category);

        Task UpdateCategoryAsync(Guid identifier, CategoryUpdateDto category);

        Task<List<CategoryDetails>> GetCategoryListAsync();

        Task RemoveCategoryAsync(Guid identifier);
    }
}
