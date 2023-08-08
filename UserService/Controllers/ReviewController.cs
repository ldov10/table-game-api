using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using UserService.Interfaces.Services;
using UserService.Models.Dto;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("postReview/userIdentifier/{userIdentifier}")]
        public async Task<IActionResult> PostReview(Guid userIdentifier, ReviewCreationDto review)
        {
            await _reviewService.CreateReviewAsync(userIdentifier, review);
            return Ok();
        }

        [HttpGet("getProductReviews/{identifier}")]
        public async Task<IActionResult> GetProductReviews(Guid identifier)
        {
            return Ok(await _reviewService.GetProductReviewsAsync(identifier));
        }

        [HttpDelete("deleteReview/{identifier}/userIdentifier/{userIdentifier}")]
        public async Task<IActionResult> DeleteReview(Guid identifier, Guid userIdentifier)
        {
            await _reviewService.RemoveReviewAsync(identifier, userIdentifier);
            return Ok();
        }
    }
}
