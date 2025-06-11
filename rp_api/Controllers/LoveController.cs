using Microsoft.AspNetCore.Mvc;
using rp_api.DTO;
using rp_api.Model;
using rp_api.Service;
using Ganss;
using Ganss.Xss;

namespace rp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoveController : Controller
    {
        private readonly ILoveService _loveService;
        private readonly HtmlSanitizer _htmlSanitizer;

        public LoveController(ILoveService loveService, HtmlSanitizer htmlSanitizer)
        {
            _loveService = loveService;
            _htmlSanitizer = htmlSanitizer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoveMessage([FromBody] LoveMessage message)
        {
            message.Message = _htmlSanitizer.Sanitize(message.Message);
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
