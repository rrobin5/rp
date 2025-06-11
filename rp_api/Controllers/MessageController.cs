using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rp_api.DTO;
using rp_api.Service;

namespace rp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly HtmlSanitizer _htmlSanitizer;

        public MessageController(IMessageService messageService, HtmlSanitizer htmlSanitizer)
        {
            _messageService = messageService;
            _htmlSanitizer = htmlSanitizer;
        }

        [Authorize(Policy = "UserNamePolicy")]
        [HttpPost("{userId}")]
        public async Task<IActionResult> SendMessage(MessageRequest messageRequest)
        {
            messageRequest.SenderUsername = _htmlSanitizer.Sanitize(messageRequest.SenderUsername);
            messageRequest.RecipientUsername = _htmlSanitizer.Sanitize(messageRequest.RecipientUsername);
            messageRequest.Content = _htmlSanitizer.Sanitize(messageRequest.Content);

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != messageRequest.SenderUsername) throw new UnauthorizedAccessException();
            await _messageService.SendMessage(messageRequest);
            return Ok("Message sent successfully.");
        }

        [Authorize(Policy = "UserNamePolicy")]
        [HttpPost("mass-message")]
        public async Task<IActionResult> SendMassMessage(MassMessageRequest messageRequest)
        {
            messageRequest.SenderUsername = _htmlSanitizer.Sanitize(messageRequest.SenderUsername);
            messageRequest.Content = _htmlSanitizer.Sanitize(messageRequest.Content);

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != "dita") throw new UnauthorizedAccessException();
            await _messageService.SendMassMessage(messageRequest);
            return Ok("Message sent successfully.");
        }

        [Authorize(Policy = "UserNamePolicy")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMessages(string username, int page = 0, int pageSize = 5)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != username) throw new UnauthorizedAccessException();
            PagedMessagesResponse messages = await _messageService.GetMessages(username, page, pageSize);
            return Ok(messages);
        }

        [Authorize(Policy = "UserNamePolicy")]
        [HttpGet("{userId}/sent")]
        public async Task<IActionResult> GetSentMessages(string username, int page = 0, int pageSize = 5)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != username) throw new UnauthorizedAccessException();
            PagedMessagesResponse messages = await _messageService.GetSentMessages(username, page, pageSize);
            return Ok(messages);
        }

        [Authorize(Policy = "UserNamePolicy")]
        [HttpGet("{userId}/unread")]
        public async Task<IActionResult> GetUnreadMessages(string username)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != username) throw new UnauthorizedAccessException();
            int unreadMessages = await _messageService.GetUnreadMessages(username);
            return Ok(unreadMessages);
        }

        [Authorize(Policy = "UserNamePolicy")]
        [HttpPatch("{userId}/read")]
        public async Task<IActionResult> MarkAsRead(string messageId, string username)
        {
            messageId = _htmlSanitizer.Sanitize(messageId);
            username = _htmlSanitizer.Sanitize(username);

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != username) throw new UnauthorizedAccessException();
            await _messageService.MarkAsRead(messageId);
            return Ok("Message marked as read");
        }
    }
}
