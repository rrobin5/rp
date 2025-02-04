using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace rp_api.Token
{
    public class UserIdClaimHandler : AuthorizationHandler<UserIdClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIdClaimRequirement requirement)
        {
            var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim != null && userIdClaim == requirement.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
