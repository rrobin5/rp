using AutoMapper;
using MongoDB.Bson;
using rp_api.DTO;
using rp_api.Model;
using rp_api.Repository;

namespace rp_api.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessageService (IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<List<MessageResponse>> GetMessages(string username, int page, int pageSize)
        {
            if (!await _userRepository.UsernameExistsAsync(username))
                throw new KeyNotFoundException("Username not found");
            List<Message> messages = await _messageRepository.GetMessages(username, page, pageSize);
            List<MessageResponse> messageResponses = _mapper.Map<List<MessageResponse>>(messages);
            return messageResponses;
        }

        public async Task<List<MessageResponse>> GetSentMessages(string username, int page, int pageSize)
        {
            if (!await _userRepository.UsernameExistsAsync(username))
                throw new KeyNotFoundException("Username not found");
            List <Message> messages = await _messageRepository.GetSentMessages(username, page, pageSize);
            List<MessageResponse> messageResponses = _mapper.Map<List<MessageResponse>>(messages);
            return messageResponses;
        }

        public async Task<int> GetUnreadMessages(string username)
        {
            if (!await _userRepository.UsernameExistsAsync(username))
                throw new KeyNotFoundException("Username not found");
            return await _messageRepository.GetUnreadMessages(username);
        }

        public async Task MarkAsRead(string messageId)
        {
            ObjectId id = ObjectId.Parse(messageId);
            await _messageRepository.MarkAsRead(id);
        }

        public async Task SendMessage(MessageRequest messageRequest)
        {
            if (!await _userRepository.UsernameExistsAsync(messageRequest.SenderUsername) 
                || !await _userRepository.UsernameExistsAsync(messageRequest.RecipientUsername))
                throw new KeyNotFoundException("Username not found");
            Message message = _mapper.Map<Message>(messageRequest);
            await _messageRepository.SendMessage(message);
        }

        public async Task SendMassMessage(MassMessageRequest messageRequest)
        {
            List<string> usernames = await _userRepository.GetAllUsernames();
            foreach (string username in usernames)
            {
                Message message = _mapper.Map<Message>(messageRequest);
                message.RecipientUsername = username;
                message.DateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await _messageRepository.SendMessage(message);
            };
        }
    }
}
