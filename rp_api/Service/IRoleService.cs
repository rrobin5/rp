using rp_api.DTO;

namespace rp_api.Service
{
    public interface IRoleService
    {
        Task<bool> AddRole(string userId, RoleRequest newRole);
        Task<bool> SaveAllRoles(string userId, List<CompleteRoleRequest> allRoles); 
        Task<bool> UpdateLastSave (string userId, long lastSave);
        Task<List<RoleResponse>> GetNotRepliedRolesByUserId(string userId);
        Task<List<RoleResponse>> GetRepliedRolesByUserId(string userId);
        Task<List<RoleResponse>> GetAllRolesByUserId(string userId);
        Task<long> GetLastSaved(string userId); 
        Task<bool> RemoveRole(string userId, string roleId);
        Task<bool> UpdateRole(RoleUpdateRequest roleUpdateRequest, string roleId, string userId);
        Task<bool> ToggleRoleStatus(string userId, string roleId);
    }
}