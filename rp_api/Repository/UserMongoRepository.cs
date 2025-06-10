using Microsoft.Extensions.Options;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.Model;
using rp_api.Service;
using System.Security.Cryptography;
using System.Text;

namespace rp_api.Repository
{
    public class UserMongoRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserMongoRepository(IOptions<MongoSettings> mongoSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoSettings.Value.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            _users = database.GetCollection<User>("Users");
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _users.Find(u => u.Username == username).AnyAsync();
        }

        public async Task CreateUserAsync(User usuario)
        {
            await _users.InsertOneAsync(usuario);
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetAllUsernames()
        {
            var usernames = await _users.Find(_ => true)
                                        .Project(u => u.Username)
                                        .ToListAsync();
            return usernames;
        }


    }

}
