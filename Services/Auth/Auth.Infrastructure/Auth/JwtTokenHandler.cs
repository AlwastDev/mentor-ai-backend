using System.IdentityModel.Tokens.Jwt;
using Auth.Application.Contracts.Auth;

namespace Auth.Infrastructure.Auth;

public sealed class JwtTokenHandler : IJwtTokenHandler
{
	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

	public JwtTokenHandler()
	{
		_jwtSecurityTokenHandler ??= new();
	}

	public string WriteToken(JwtSecurityToken jwt) => _jwtSecurityTokenHandler.WriteToken(jwt);
}