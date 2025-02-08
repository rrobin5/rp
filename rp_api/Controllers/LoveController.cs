using Microsoft.AspNetCore.Mvc;
using rp_api.DTO;
using rp_api.Model;
using rp_api.Service;

namespace rp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoveController : Controller
    {
        private readonly ILoveService _loveService;

        public LoveController(ILoveService loveService)
        {
            _loveService = loveService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoveMessage([FromBody] LoveMessage message)
        {
            await _loveService.CreateMessage(message);
            return Ok("Message created successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            List<LoveMessageResponse> messages = await _loveService.GetAllMessages();
            return Ok(messages);
        }
    }
}
