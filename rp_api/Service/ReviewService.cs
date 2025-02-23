using AutoMapper;
using rp_api.DTO;
using rp_api.Model;
using rp_api.Repository;

namespace rp_api.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService (IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task CreateReview(ReviewRequest reviewRequest)
        {
            Review newReview = _mapper.Map<Review>(reviewRequest);
            await _reviewRepository.CreateReview(newReview);
        }

        public async Task<List<ReviewResponse>> GetAllReviews()
        {
            List<Review> reviews = await _reviewRepository.GetAllReviews();
            return _mapper.Map<List<ReviewResponse>>(reviews);
        }
    }
}
