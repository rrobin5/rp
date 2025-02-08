
using AutoMapper;
using MongoDB.Bson;
using rp_api.DTO;
using rp_api.Model;
using rp_api.Repository;
using System.Data;

namespace rp_api.Service
{
    public class LoveService : ILoveService
    {
        private readonly ILoveRepository _loveRepository;
        private readonly IMapper _mapper;
        public LoveService(ILoveRepository loveRepository, IMapper mapper)
        {
            _loveRepository = loveRepository;
            _mapper = mapper;
        }

        public async Task CreateMessage(LoveMessage message)
        {
            message.Id = ObjectId.GenerateNewId();
            await _loveRepository.CreateMessageAsync(message);
        }

        public async Task<List<LoveMessageResponse>> GetAllMessages()
        {
            List<LoveMessage> messages = await _loveRepository.GetAllMessagesAsync();
            return _mapper.Map<List<LoveMessageResponse>>(messages);
        }
    }
}
