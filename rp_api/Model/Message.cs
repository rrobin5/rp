using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace rp_api.Model
{
    public class Message
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public long DateTime { get; set; }
        public bool Read {  get; set; } = false;
        public long ReadDateTime { get; set; }
    }
}
