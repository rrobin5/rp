
using rp_api.Model;
using rp_api.Repository;

namespace rp_api.Service
{
    public class LoveService : ILoveService
    {
        private readonly ILoveRepository _loveRepository;

        public LoveService(ILoveRepository loveRepository)
        {
            _loveRepository = loveRepository;
        }

        public async Task CreateMessage(LoveMessage message)
        {
            await _loveRepository.CreateMessageAsync(message);
        }

        public async Task<List<LoveMessage>> GetAllMessages()
        {
            return await _loveRepository.GetAllMessagesAsync();
        }
    }
}
