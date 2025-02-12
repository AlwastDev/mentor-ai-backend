using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Auth;

public class JwtIssuerOptions
{
	/// <summary>
	/// 4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
	/// </summary>
	public string Issuer { get; set; }

	/// <summary>
	/// 4.1.3.  "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
	/// </summary>
	public string Audience { get; set; }

	public int AccessTokenDurationMinutes { get; set; }
	public int RefreshTokenDurationMinutes { get; set; }
	public int RegistrationTokenDurationMinutes { get; set; }

	/// <summary>
	/// The signing key to use when generating tokens.
	/// </summary>
	public SigningCredentials SigningCredentials { get; set; }
	public string SecretKey { get; set; }
}