
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {

            await _userService.CreateUser(userRequest);

            return Ok("User registered successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginResponse loginResponse = await _userService.Login(loginRequest);
            if (loginResponse == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(loginResponse);
        }

    }
}

