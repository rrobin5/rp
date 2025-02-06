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
            role.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            role.Id = ObjectId.GenerateNewId();

            ObjectId objectId = ObjectId.Parse(userId);

            return await _roleRepository.AddRoleAsync(objectId, role);

        }

        public async Task<bool> SaveAllRoles(string userId, List<AllRolesRequest> allRoles)
        {

            ObjectId objectUserId = ObjectId.Parse(userId);

            foreach (AllRolesRequest roleRequest in allRoles)
            {
                Role role = _mapper.Map<Role>(roleRequest);
                bool success = await _roleRepository.UpdateRoleAsync(userId, roleRequest.Id, role);
                if (!success)
                {
                    success = await _roleRepository.AddRoleAsync(objectUserId, role);
                    if (!success) return false;
                }

            }

            return true;
        }

        public async Task<List<RoleResponse>> GetNotRepliedRolesByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                throw new KeyNotFoundException("User not found");
            }
            var roles = await _roleRepository.GetNotRepliedRolesByUserIdAsync(userObjectId);

            roles = roles
                .OrderBy(r => r.TimeStamp)
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
                .OrderByDescending(r => r.TimeStamp)
                .ToList();

            return _mapper.Map<List<RoleResponse>>(roles);

        }

        public async Task<List<RoleResponse>> GetAllRolesByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                throw new KeyNotFoundException("User not found");
            }

            var roles = await _roleRepository.GetAllRolesByUserIdAsync(userObjectId);

            var roleResponse = _mapper.Map<List<RoleResponse>>(roles);

            return roleResponse;
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
