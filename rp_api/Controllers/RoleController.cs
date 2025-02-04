
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rp_api.DTO;
using rp_api.Service;

namespace rp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetRolesByUserId(string userId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            var (notReplied, replied) = await _roleService.GetAllRolesByUserId(userId);

            return Ok(new
            {
                NotReplied = notReplied,
                Replied = replied
            });

        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpGet("{userId}/not-replied-roles")]
        public async Task<IActionResult> GetNotRepliedRolesByUserId(string userId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            List<RoleResponse> roles = await _roleService.GetNotRepliedRolesByUserId(userId);

            return Ok(roles);

        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpGet("{userId}/replied-roles")]
        public async Task<IActionResult> GetRepliedRolesByUserId(string userId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            List<RoleResponse> roles = await _roleService.GetRepliedRolesByUserId(userId);

            return Ok(roles);

        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequest roleRequest, string userId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            bool success = await _roleService.AddRole(userId, roleRequest);

            if (success) return Ok("Rol added successfully.");
             return NotFound("User not found or failed to add rol.");
        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpPut("{userId}/{roleId}")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateRequest roleUpdateRequest, string roleId, string userId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            bool success = await _roleService.UpdateRole(roleUpdateRequest, roleId, userId);

            if (success)return Ok("Rol updated successfully.");
            return NotFound("User or role not found.");

        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> RemoveRole(string roleId, string userId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            bool success = await _roleService.RemoveRole(userId, roleId);

            if (success) return Ok("Rol removed successfully.");
            return NotFound("User or rol not found.");

        }

        [Authorize(Policy = "UserIdPolicy")]
        [HttpPatch("{userId}/toggle-status/{roleId}")]
        public async Task<IActionResult> ToggleRoleStatus(string userId, string roleId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim != userId) throw new UnauthorizedAccessException();

            var success = await _roleService.ToggleRoleStatus(userId, roleId);
            if (!success) return NotFound(new { message = "User or rol not found." });
            return Ok(new { message = "Rol status updated successfully." });
        }
    }

}
