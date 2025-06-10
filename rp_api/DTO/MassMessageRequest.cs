using System.ComponentModel.DataAnnotations;

namespace rp_api.DTO
{
    public class MassMessageRequest
    {
        [Required(ErrorMessage = "Sender is required.")]
        public string SenderUsername { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
    }
}
