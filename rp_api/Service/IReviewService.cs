using rp_api.DTO;

namespace rp_api.Service
{
    public interface IReviewService
    {
        Task CreateReview(ReviewRequest reviewRequest);
        Task<List<ReviewResponse>> GetAllReviews();
    }
}
