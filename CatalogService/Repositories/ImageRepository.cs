using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatalogService.Context;
using CatalogService.Interfaces.Repositories;
using CatalogService.Models.Entities;
using System;

namespace CatalogService.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ServiceDbContext _context;

        public ImageRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task SaveImageAsync(Image image)
        {
            await _context.Images.AddAsync(image);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductImagesAsync(long productId)
        {
            var userImages = await _context.Images
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            _context.Images.RemoveRange(userImages);

            await _context.SaveChangesAsync();
        }

        public async Task<Image> GetProductImageAsync(Guid identifier)
        {
            return await _context.Images
                .FirstOrDefaultAsync(x => x.Identifier == identifier);
        }

        public async Task<Image> GetProductFirstImageAsync(long productId)
        {
            return await _context.Images
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<Guid>> GetProductImagesIdsAsync(long productId)
        {
            return await _context.Images
                .Where(x => x.ProductId == productId)
                .Select(x => x.Identifier)
                .ToListAsync();
        }
    }
}
