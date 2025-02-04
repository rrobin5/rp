using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace rp_api.Model
{
    public class Role
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Characters { get; set; }
        public string Partner { get; set; }
        public string Link { get; set; }
        public RoleStatus Status { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string TimeStamp {  get; set; } 

    }
}
