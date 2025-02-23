using Microsoft.AspNetCore.Mvc;
using rp_api.DTO;
using rp_api.Service;

namespace rp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController (IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewRequest reviewRequest)
        {
            await _reviewService.CreateReview(reviewRequest);
            return Ok("Message created successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            List<ReviewResponse> reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }
    }
}
