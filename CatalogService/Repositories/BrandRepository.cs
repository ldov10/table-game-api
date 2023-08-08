using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Context;
using CatalogService.Interfaces.Repositories;
using CatalogService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ServiceDbContext _context;

        public BrandRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Brand> GetBrandAsync(Guid identifier)
        {
            return await _context.Brands.FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task<Brand> GetBrandAsync(string title)
        {
            title = title.Trim().ToUpper();

            return await _context.Brands.FirstOrDefaultAsync(x => string.Equals(x.Title, title));
        }

        public async Task SaveBrandAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            _context.Brands.Update(brand);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Brand>> GetBrandListAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task RemoveBrandAsync(Brand brand)
        {
            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();
        }
    }
}
