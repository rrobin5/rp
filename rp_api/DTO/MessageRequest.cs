using System.ComponentModel.DataAnnotations;

namespace rp_api.DTO
{
    public class MessageRequest
    {
        [Required(ErrorMessage = "Sender is required.")]
        public string SenderUsername { get; set; }

        [Required(ErrorMessage = "Recipient is required.")]
        public string RecipientUsername { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public long DateTime { get; set; }
    }
}
