
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using rp_api.DTO;
using rp_api.Service;

namespace rp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly HtmlSanitizer _htmlSanitizer;

        public UserController(IUserService userService, HtmlSanitizer htmlSanitizer)
        {
            _userService = userService;
            _htmlSanitizer = htmlSanitizer;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            userRequest.Username = _htmlSanitizer.Sanitize(userRequest.Username);
            userRequest.Password = _htmlSanitizer.Sanitize(userRequest.Password);
            await _userService.CreateUser(userRequest);

            return Ok("User registered successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            loginRequest.Username = _htmlSanitizer.Sanitize(loginRequest.Username);
            loginRequest.Password = _htmlSanitizer.Sanitize(loginRequest.Password);
            LoginResponse loginResponse = await _userService.Login(loginRequest);
            if (loginResponse == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(loginResponse);
        }

    }
}

