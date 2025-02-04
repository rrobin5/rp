using Microsoft.IdentityModel.Tokens;
using rp_api.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace rp_api.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(User usuario)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", "user"),
            new Claim("id", usuario.Id.ToString())
        };

            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? _configuration["Jwt:Issuer"];

            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                              ?? _configuration["Jwt:Audience"];

            var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                               ?? _configuration["Jwt:SecretKey"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
