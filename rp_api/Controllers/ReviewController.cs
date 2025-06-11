using Ganss.Xss;
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
        private readonly HtmlSanitizer _htmlSanitizer;
        public ReviewController (IReviewService reviewService, HtmlSanitizer htmlSanitizer)
        {
            _reviewService = reviewService;
            _htmlSanitizer = htmlSanitizer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewRequest reviewRequest)
        {
            reviewRequest.UserId = _htmlSanitizer.Sanitize(reviewRequest.UserId);
            reviewRequest.Message = _htmlSanitizer.Sanitize(reviewRequest.Message);
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
