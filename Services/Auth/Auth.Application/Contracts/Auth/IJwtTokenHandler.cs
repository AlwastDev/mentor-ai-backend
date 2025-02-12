using System.IdentityModel.Tokens.Jwt;

namespace Auth.Application.Contracts.Auth;

public interface IJwtTokenHandler
{
    string WriteToken(JwtSecurityToken jwt);
}