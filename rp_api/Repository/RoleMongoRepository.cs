using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.Model;

namespace rp_api.Repository
{
    public class RoleMongoRepository : IRoleRepository
    {
        private readonly IMongoCollection<User> _roles;

        public RoleMongoRepository(IOptions<MongoSettings> mongoSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoSettings.Value.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            _roles = database.GetCollection<User>("Users");
        }

        public async Task<bool> AddRoleAsync(ObjectId userId, Role newRole)
        {

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Push(u => u.Roles, newRole);

            var updateResult = await _roles.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;

        }

        public async Task<bool> UpdateRoleAsync(string userId, string roleId, Role updatedRole)
        {
            if (!ObjectId.TryParse(userId, out var usuarioObjectId))
            {
                return false;
            }

            if (!ObjectId.TryParse(roleId, out var rolObjectId))
            {
                return false;
            }

            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Id, usuarioObjectId),
                Builders<User>.Filter.ElemMatch(u => u.Roles, r => r.Id == rolObjectId)
            );

            var update = Builders<User>.Update.Combine(
                Builders<User>.Update.Set("Roles.$.Title", updatedRole.Title),
                Builders<User>.Update.Set("Roles.$.Characters", updatedRole.Characters),
                Builders<User>.Update.Set("Roles.$.Partner", updatedRole.Partner),
                Builders<User>.Update.Set("Roles.$.Link", updatedRole.Link),
                Builders<User>.Update.Set("Roles.$.Status", updatedRole.Status),
                Builders<User>.Update.Set("Roles.$.TimeStamp", updatedRole.TimeStamp)
            );

            var updateResult = await _roles.UpdateOneAsync(filter, update);

            return updateResult.MatchedCount > 0;
        }

        public async Task<List<Role>> GetNotRepliedRolesByUserIdAsync(ObjectId userId)
        {
            var roles = await _roles
                .Find(u => u.Id == userId)
                .Project(u => u.Roles
                    .Where(r => r.Status == RoleStatus.NotReplied)
                    .ToList())
                .FirstOrDefaultAsync();

            return (roles);
        }

        public async Task<List<Role>> GetRepliedRolesByUserIdAsync(ObjectId userId)
        {
            var roles = await _roles
                .Find(u => u.Id == userId)
                .Project(u => u.Roles
                    .Where(r => r.Status == RoleStatus.Replied)
                    .ToList())
                .FirstOrDefaultAsync();

            return (roles);
        }

        public async Task<List<Role>> GetAllRolesByUserIdAsync(ObjectId userId)
        {
            var user = await _roles.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) return new List<Role>();

            var roles= user.Roles
                .ToList();

            return roles;
        }

        public async Task<bool> RemoveRoleAsync(string userId, string roleId)
        {
            if (!ObjectId.TryParse(userId, out var usuarioObjectId))
            {
                return false;
            }

            if (!ObjectId.TryParse(roleId, out var rolObjectId))
            {
                return false;
            }

            var filter = Builders<User>.Filter.Eq(u => u.Id, usuarioObjectId);
            var update = Builders<User>.Update.PullFilter(u => u.Roles, r => r.Id == rolObjectId);

            var updateResult = await _roles.UpdateOneAsync(filter, update);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> ToggleRoleStatusAsync(ObjectId userObjectId, ObjectId roleObjectId)
        {

            User user = await _roles.Find(u => u.Id == userObjectId).FirstOrDefaultAsync();
            if (user == null) return false;

            Role role = user.Roles.FirstOrDefault(r => r.Id == roleObjectId);
            if (role == null) return false;

            var newStatus = role.Status == 0 ? 1 : 0;

            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Id, userObjectId),
                Builders<User>.Filter.ElemMatch(u => u.Roles, r => r.Id == roleObjectId)
            );



            var update = Builders<User>.Update.Combine(
                Builders<User>.Update.Set("Roles.$.Status", newStatus),
                Builders<User>.Update.Set("Roles.$.TimeStamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                );

            var updateResult = await _roles.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }
    }
}
