using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models.Entities;

namespace UserService.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetUserReviewsAsync(long userId);

        Task SaveReviewAsync(Review review);

        Task<List<Review>> GetProductReviewsAsync(Guid productIdentifier);

        Task<Review> GetReviewAsync(Guid identifier);

        Task RemoveReviewAsync(Review review);

        Task<int> GetProductRatingsCountAsync(Guid productIdentifier);

        Task<int> GetProductRatingsSumAsync(Guid productIdentifier);
    }
}
