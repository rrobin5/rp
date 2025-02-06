using rp_api.Model;

namespace rp_api.DTO
{
    public class RoleUpdateRequest
    {
        public string Title { get; set; }
        public string Characters { get; set; }
        public string Partner { get; set; }
        public string Link { get; set; }
        public RoleStatus Status { get; set; }
        public long TimeStamp { get; set; }
    }
}
