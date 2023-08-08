using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Context;
using UserService.Interfaces.Repositories;
using UserService.Models.Entities;

namespace UserService.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ServiceDbContext _context;

        public ReviewRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetUserReviewsAsync(long userId)
        {
            return await _context.Reviews.Where(x => x.UserId == userId && x.User.IsActive).ToListAsync();
        }

        public async Task SaveReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetProductReviewsAsync(Guid productIdentifier)
        {
            return await _context.Reviews
                .Include(x => x.User)
                .Where(x => x.ProductIdentifier == productIdentifier && x.User.IsActive).ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Guid identifier)
        {
            return await _context.Reviews
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Identifier == identifier && x.User.IsActive);
        }

        public async Task RemoveReviewAsync(Review review)
        {
            _context.Reviews.Remove(review);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetProductRatingsCountAsync(Guid productIdentifier)
        {
            return await _context.Reviews
                .Where(x => x.ProductIdentifier == productIdentifier && x.User.IsActive)
                .CountAsync();
        }

        public async Task<int> GetProductRatingsSumAsync(Guid productIdentifier)
        {
            return await _context.Reviews
                .Where(x => x.ProductIdentifier == productIdentifier && x.User.IsActive)
                .SumAsync(x => x.Rating);
        }
    }
}
