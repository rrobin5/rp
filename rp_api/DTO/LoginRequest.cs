using System.ComponentModel.DataAnnotations;

namespace rp_api.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
