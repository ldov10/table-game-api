using CatalogService.Models.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CatalogService.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryAsync(Guid identifier);

        Task<Category> GetCategoryAsync(string title);

        Task SaveCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);

        Task<List<Category>> GetCategoryListAsync();

        Task RemoveCategoryAsync(Category category);
    }
}
