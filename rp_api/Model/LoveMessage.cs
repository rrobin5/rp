using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace rp_api.Model
{
    public class LoveMessage
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Message { get; set; }
    }
}
