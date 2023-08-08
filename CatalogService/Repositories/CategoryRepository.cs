using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Context;
using CatalogService.Interfaces.Repositories;
using CatalogService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ServiceDbContext _context;

        public CategoryRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryAsync(Guid identifier)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task<Category> GetCategoryAsync(string title)
        {
            title = title.Trim().ToUpper();

            return await _context.Categories.FirstOrDefaultAsync(x => string.Equals(x.Title, title));
        }

        public async Task SaveCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoryListAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }
    }
}
