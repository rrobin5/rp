using Microsoft.AspNetCore.Authorization;

namespace rp_api.Token
{
    public class UserIdClaimRequirement : IAuthorizationRequirement
    {
        public string UserId { get; set; }
        public UserIdClaimRequirement(string userId)
        {
            UserId = userId;
        }
    }
}
