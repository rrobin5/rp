using rp_api.Model;

namespace rp_api.DTO
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
        public long LastSave { get; set; }
    }
}
