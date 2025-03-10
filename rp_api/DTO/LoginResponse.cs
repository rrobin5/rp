namespace rp_api.DTO
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public long LastSave { get; set; }
    }
}
