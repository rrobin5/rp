using Microsoft.Extensions.Options;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.Model;

namespace rp_api.Repository
{
    public class LoveMongoRepository : ILoveRepository
    {
        private readonly IMongoCollection<LoveMessage> _messages;

        public LoveMongoRepository(IOptions<MongoSettings> mongoSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoSettings.Value.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            _messages = database.GetCollection<LoveMessage>("sanvalentin");
        }

        public async Task CreateMessageAsync(LoveMessage message)
        {
            await _messages.InsertOneAsync(message);
        }
        public async Task<List<LoveMessage>> GetAllMessagesAsync()
        {
            return await _messages.Find(_ => true).ToListAsync();
        }
    }
}
