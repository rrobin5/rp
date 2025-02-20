using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace rp_api.Model
{
    public class Review
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
