using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.Model;

namespace rp_api.Repository
{
    public class MessageMongoRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageMongoRepository(IOptions<MongoSettings> mongoSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoSettings.Value.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            _messages = database.GetCollection<Message>("Messages");
        }
        public async Task<List<Message>> GetMessages(string username, int page, int pageSize)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.RecipientUsername, username);

            var messages = await _messages.Find(filter)
                .SortByDescending(m => m.DateTime)
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return messages;
        }

        public async Task<List<Message>> GetSentMessages(string username, int page = 0, int pageSize = 5)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.SenderUsername, username);

            var messages = await _messages.Find(filter)
                .SortByDescending(m => m.DateTime)
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return messages;
        }

        public async Task<int> GetUnreadMessages(string username)
        {
            var filter = Builders<Message>.Filter.And(
                Builders<Message>.Filter.Eq(m => m.RecipientUsername, username),
                Builders<Message>.Filter.Eq(m => m.Read, false)
            );

            return (int)await _messages.CountDocumentsAsync(filter);
        }

        public async Task MarkAsRead(ObjectId messageId)
        {
            var update = Builders<Message>.Update
                            .Set(m => m.Read, true)
                            .Set(m => m.ReadDateTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            var result = await _messages.UpdateOneAsync(
                Builders<Message>.Filter.Eq(m => m.Id, messageId),
                update
            );
        }

        public async Task SendMessage(Message message)
        {
            _messages.InsertOne(message);
        }
    }
}
