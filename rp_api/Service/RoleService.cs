using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using rp_api.DTO;
using rp_api.Model;
using rp_api.Repository;

namespace rp_api.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddRole(string userId, RoleRequest newRole)
        {
            Role role = _mapper.Map<Role>(newRole);
            role.Status = 0;
            role.TimeStamp = DateTime.Now.ToString("o");
            role.Id = ObjectId.GenerateNewId();

            ObjectId objectId = ObjectId.Parse(userId);

            return await _roleRepository.AddRoleAsync(objectId, role);

        }

        public async Task<List<RoleResponse>> GetNotRepliedRolesByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                throw new KeyNotFoundException("User not found");
            }
            var roles = await _roleRepository.GetNotRepliedRolesByUserIdAsync(userObjectId);

            roles = roles
                .OrderBy(r => DateTime.Parse(r.TimeStamp))
                .ToList();

            return _mapper.Map<List<RoleResponse>>(roles);

        }

        public async Task<List<RoleResponse>> GetRepliedRolesByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                throw new KeyNotFoundException("User not found");
            }
            var roles = await _roleRepository.GetRepliedRolesByUserIdAsync(userObjectId);

            roles = roles
                .OrderByDescending(r => DateTime.Parse(r.TimeStamp))
                .ToList();

            return _mapper.Map<List<RoleResponse>>(roles);

        }

        public async Task<(List<RoleResponse> notRepliedRoles, List<RoleResponse> repliedRoles)> GetAllRolesByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                throw new KeyNotFoundException("User not found");
            }

            var roles = await _roleRepository.GetAllRolesByUserIdAsync(userObjectId);

            var notReplied = roles
                .Where(r => r.Status == RoleStatus.NotReplied)
                .OrderBy(r => DateTime.Parse(r.TimeStamp))
                .ToList();

            var replied = roles
                .Where(r => r.Status == RoleStatus.Replied)
                .OrderByDescending(r => DateTime.Parse(r.TimeStamp))
                .ToList();

            var notRepliedRoles = _mapper.Map<List<RoleResponse>>(notReplied);
            var repliedRoles = _mapper.Map<List<RoleResponse>>(replied);

            return (notRepliedRoles, repliedRoles);
        }

        public async Task<bool> UpdateRole(RoleUpdateRequest roleUpdateRequest, string roleId, string userId)
        {
            var role = _mapper.Map<Role>(roleUpdateRequest);

            return await _roleRepository.UpdateRoleAsync(userId, roleId, role);
        }

        public async Task<bool> RemoveRole(string userId, string roleId)
        {
            return await _roleRepository.RemoveRoleAsync(userId, roleId);
        }

        public async Task<bool> ToggleRoleStatus(string userId, string roleId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId) || !ObjectId.TryParse(roleId, out var roleObjectId))
            {
                return false;
            }

            return await _roleRepository.ToggleRoleStatusAsync(userObjectId, roleObjectId);
        }

    }
}
