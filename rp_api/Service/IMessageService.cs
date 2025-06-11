using MongoDB.Bson;
using rp_api.DTO;

namespace rp_api.Service
{
    public interface IMessageService
    {
        Task SendMessage(MessageRequest messageRequest);
        Task<PagedMessagesResponse> GetMessages(string username, int page, int pageSize);
        Task<PagedMessagesResponse> GetSentMessages(string username, int page, int pageSize);
        Task<int> GetUnreadMessages(string username);
        Task MarkAsRead(string messageId);
        Task SendMassMessage(MassMessageRequest messageRequest);
    }
}
