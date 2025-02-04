using rp_api.Model;

namespace rp_api.Token
{
    public interface ITokenService
    {
        string GenerateJwtToken(User usuario);
    }
}