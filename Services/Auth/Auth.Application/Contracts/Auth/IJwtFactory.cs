using System.Security.Claims;
using Auth.Domain.Common;

namespace Auth.Application.Contracts.Auth;

public interface IJwtFactory
{
    string GenerateEncodedToken(IEnumerable<Claim> claims, TokenType tokenType);
    IEnumerable<Claim> Parse(string token);
    bool Validate(string token);
}