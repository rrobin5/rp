using rp_api.DTO;

namespace rp_api.Service
{
    public interface IRoleService
    {
        Task<bool> AddRole(string userId, RoleRequest newRole);
        Task<List<RoleResponse>> GetNotRepliedRolesByUserId(string userId);
        Task<List<RoleResponse>> GetRepliedRolesByUserId(string userId);
        Task<(List<RoleResponse> notRepliedRoles, List<RoleResponse> repliedRoles)> GetAllRolesByUserId(string userId);
        Task<bool> RemoveRole(string userId, string roleId);
        Task<bool> UpdateRole(RoleUpdateRequest roleUpdateRequest, string roleId, string userId);
        Task<bool> ToggleRoleStatus(string userId, string roleId);
    }
}