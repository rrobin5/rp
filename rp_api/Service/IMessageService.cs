using MongoDB.Bson;
using rp_api.DTO;

namespace rp_api.Service
{
    public interface IMessageService
    {
        Task SendMessage(MessageRequest messageRequest);
        Task<List<MessageResponse>> GetMessages(string username, int page, int pageSize);
        Task<List<MessageResponse>> GetSentMessages(string username, int page, int pageSize);
        Task<int> GetUnreadMessages(string username);
        Task MarkAsRead(string messageId);
    }
}
