using System.Threading.Tasks;
using System;
using UserService.Models.Dto;
using System.Collections.Generic;

namespace UserService.Interfaces.Services
{
    public interface IReviewService
    {
        Task CreateReviewAsync(Guid userIdentifier, ReviewCreationDto review);

        Task<List<ProductReview>> GetProductReviewsAsync(Guid productIdentifier);

        Task RemoveReviewAsync(Guid identifier, Guid userIdentifier);
    }
}
