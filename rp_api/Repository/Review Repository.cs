using Microsoft.Extensions.Options;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.DTO;
using rp_api.Model;

namespace rp_api.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IMongoCollection<Review> _reviews;
        public ReviewRepository(IOptions<MongoSettings> mongoSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoSettings.Value.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            _reviews = database.GetCollection<Review>("Reviews");
        }
        public async Task CreateReview(Review review)
        {
            await _reviews.InsertOneAsync(review);
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _reviews.Find(_ => true).ToListAsync();
        }
    }
}
