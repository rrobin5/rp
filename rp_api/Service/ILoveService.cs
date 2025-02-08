using rp_api.DTO;
using rp_api.Model;

namespace rp_api.Service
{
    public interface ILoveService
    {
        Task CreateMessage(LoveMessage message);
        Task<List<LoveMessageResponse>> GetAllMessages();
    }
}