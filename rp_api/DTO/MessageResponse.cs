namespace rp_api.DTO
{
    public class MessageResponse
    {
        public string Id { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public long DateTime { get; set; }
        public bool Read { get; set; }
        public long ReadDateTime { get; set; }
    }
}
