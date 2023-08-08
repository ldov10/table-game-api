using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Context;
using UserService.Interfaces.Repositories;
using UserService.Models.Entities;

namespace UserService.Repositories
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

        public async Task RemoveUserImagesAsync(long userId)
        {
            var userImages = await _context.Images
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _context.Images.RemoveRange(userImages);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Image>> GetUserImagesAsync(long userId)
        {
            return await _context.Images
                .Where(x => x.UserId == userId && x.User.IsActive)
                .ToListAsync();
        }
    }
}
