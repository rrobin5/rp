using rp_api.DTO;
using rp_api.Model;

namespace rp_api.Repository
{
    public interface IReviewRepository
    {
        Task CreateReview(Review review);
        Task<List<Review>> GetAllReviews();
    }
}
