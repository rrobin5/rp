namespace rp_api.DTO
{
    public class MessageUpdateRequest
    {
        public string Id { get; set; }
        public bool Read { get; set; }
        public long ReadDateTime { get; set; }
    }
}
