using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.Contracts.Auth;
using Auth.Domain.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Auth;

public sealed class JwtFactory : IJwtFactory
{
	private readonly IJwtTokenHandler _jwtTokenHandler;
	private readonly JwtIssuerOptions _jwtOptions;

	public JwtFactory(IJwtTokenHandler jwtTokenHandler,
		IOptions<JwtIssuerOptions> jwtOptions)
	{
		_jwtTokenHandler = jwtTokenHandler;
		_jwtOptions = jwtOptions.Value;
	}

	public string GenerateEncodedToken(IEnumerable<Claim> claims, TokenType tokenType)
	{
		var expireDate = tokenType switch
		{
			TokenType.AccessToken => DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenDurationMinutes),
			TokenType.RefreshToken => DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenDurationMinutes),
			TokenType.RegistrationToken => DateTime.UtcNow.AddMinutes(_jwtOptions.RegistrationTokenDurationMinutes),
			_ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
		};

		var token = new JwtSecurityToken(
			_jwtOptions.Issuer,
			_jwtOptions.Audience,
			claims,
			expires: expireDate,
			signingCredentials: _jwtOptions.SigningCredentials);

		return _jwtTokenHandler.WriteToken(token);
	}

	public IEnumerable<Claim> Parse(string token)
	{
		var jwtToken = new JwtSecurityToken(token);

		return jwtToken.Claims;
	}

	public bool Validate(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		try
		{
			tokenHandler.ValidateToken(token, new()
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ClockSkew = TimeSpan.Zero,
				ValidateTokenReplay = true,
				ValidIssuer = _jwtOptions.Issuer,
				ValidAudience = _jwtOptions.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
			}, out _);
		}
		catch (Exception ex)
		{
			return false;
		}

		return true;
	}
}