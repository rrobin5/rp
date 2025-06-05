using MongoDB.Bson;
using rp_api.Model;

namespace rp_api.Repository
{
    public interface IMessageRepository
    {
        Task SendMessage(Message message);
        Task<List<Message>> GetMessages(string username, int page, int pageSize);
        Task<List<Message>> GetSentMessages(string username, int page, int pageSize);
        Task<int> GetUnreadMessages (string username);
        Task MarkAsRead(ObjectId messageId);
    }
}
