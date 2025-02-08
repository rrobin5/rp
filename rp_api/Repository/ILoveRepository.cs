using rp_api.Model;

namespace rp_api.Repository
{
    public interface ILoveRepository
    {
        Task CreateMessageAsync(LoveMessage message);
        Task<List<LoveMessage>> GetAllMessagesAsync();
    }
}