using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using UserService.Exceptions;
using UserService.Interfaces.Repositories;
using UserService.Interfaces.Services;
using UserService.Models.Dto;
using UserService.Models.Entities;
using UserService.Models.Messages;
using UserService.Options;

namespace UserService.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMessageSenderService _messageSenderService;
        private readonly RabbitMqQueuesOptions _rabbitMqQueuesOptions;

        public ReviewService(IMapper mapper,
            IUserRepository userRepository,
            IReviewRepository reviewRepository,
            IOptions<RabbitMqQueuesOptions> rabbitMqQueuesOptions,
            IMessageSenderService messageSenderService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _messageSenderService = messageSenderService;
            _rabbitMqQueuesOptions = rabbitMqQueuesOptions.Value;
        }

        public async Task CreateReviewAsync(Guid userIdentifier, ReviewCreationDto review)
        {
            if (review.ProductIdentifier == Guid.Empty)
            {
                throw new InternalException("Invalid ProductIdentifier.");
            }

            var user = await _userRepository.GetUserAsync(userIdentifier);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var userReviews = await _reviewRepository.GetUserReviewsAsync(user.Id);

            if (userReviews.Any(x => x.ProductIdentifier == review.ProductIdentifier))
            {
                throw new InternalException("User has a review about given product.");
            }

            review.Description = review.Description.Trim();

            var newReview = _mapper.Map<Review>(review);

            newReview.User = user;

            await _reviewRepository.SaveReviewAsync(newReview);

            await SendProductRatingChangedMessageAsync(review.ProductIdentifier);
        }

        public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productIdentifier)
        {
            var reviews = await _reviewRepository.GetProductReviewsAsync(productIdentifier);

            if (reviews == null || !reviews.Any())
            {
                return new List<ProductReview>();
            }

            var productReviews = _mapper.Map<List<ProductReview>>(reviews);

            return productReviews;
        }

        public async Task RemoveReviewAsync(Guid identifier, Guid userIdentifier)
        {
            var review = await _reviewRepository.GetReviewAsync(identifier);

            if (review == null)
            {
                throw new NotFoundException("Review not found.");
            }

            if (review.User.Identifier != userIdentifier)
            {
                throw new InternalException("User verification error.");
            }

            await _reviewRepository.RemoveReviewAsync(review);

            await SendProductRatingChangedMessageAsync(review.ProductIdentifier);
        }

        private async Task SendProductRatingChangedMessageAsync(Guid productIdentifier)
        {
            var productRatingsCount = await _reviewRepository.GetProductRatingsCountAsync(productIdentifier);

            if (productRatingsCount == 0)
            {
                var message = new ProductRatingChangedMessage
                {
                    ProductIdentifier = productIdentifier,
                    Rating = 0
                };
                _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CatalogService);
            }
            else
            {
                var productRatingsSum = await _reviewRepository.GetProductRatingsSumAsync(productIdentifier);
                var productRating = (double)productRatingsSum / productRatingsCount;

                var message = new ProductRatingChangedMessage
                {
                    ProductIdentifier = productIdentifier,
                    Rating = Math.Round(productRating, 2)
                };
                _messageSenderService.SendMessage(message, _rabbitMqQueuesOptions.CatalogService);
            }
        }
    }
}
