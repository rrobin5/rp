namespace rp_api.DTO
{
    public class PagedMessagesResponse
    {
        public List<MessageResponse> Messages { get; set; }
        public bool HasMore { get; set; }
    }
}
