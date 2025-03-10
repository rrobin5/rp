using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace rp_api.Model
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
        public long LastSave {  get; set; }
    }
}
