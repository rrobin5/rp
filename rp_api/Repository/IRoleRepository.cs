using MongoDB.Bson;
using rp_api.DTO;
using rp_api.Model;

namespace rp_api.Repository
{
    public interface IRoleRepository
    {
        Task<bool> AddRoleAsync(ObjectId userId, Role newRole);
        Task<bool> UpdateRoleAsync(string userId, string roleId, Role updatedRole);
        Task<bool> RemoveRoleAsync(string userId, string roleId);
        Task<List<Role>> GetAllRolesByUserIdAsync(ObjectId userId);
        Task<List<Role>> GetNotRepliedRolesByUserIdAsync(ObjectId userId);
        Task<List<Role>> GetRepliedRolesByUserIdAsync(ObjectId userId);
        Task<long> GetLastSaved(ObjectId userId);
        Task<bool> ToggleRoleStatusAsync(ObjectId userId, ObjectId roleId);
        Task<bool> ReplaceAllRolesAsync(string userId, List<Role> newRoles);
        Task<bool> UpdateLastSaved(long lastSaved, ObjectId userId);
    }
}