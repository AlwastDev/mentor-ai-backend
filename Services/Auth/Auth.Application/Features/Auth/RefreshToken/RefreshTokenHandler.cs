using System.Security.Claims;
using Auth.Application.Contracts.Auth;
using Auth.Application.Contracts.Persistence;
using Auth.Application.Responses;
using Auth.Domain.Common;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ResultResponse<AccessTokenResponse>>
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtFactory _jwtFactory;

    public RefreshTokenHandler(IAuthRepository authRepository, IJwtFactory jwtFactory)
    {
        _authRepository = authRepository;
        _jwtFactory = jwtFactory;
    }

    public async Task<ResultResponse<AccessTokenResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (!_jwtFactory.Validate(request.Token))
        {
            return new(ResultCode.Forbidden);
        }

        var claims = _jwtFactory.Parse(request.Token);
        var userId = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return new(ResultCode.Forbidden);
        }

        var user = await _authRepository.GetByIdAsync(userId);

        if (user?.IsAuthTokenIssued != true)
        {
            return new(ResultCode.UnAuthorize, "Token is no longer valid");
        }

        await _authRepository.UpdateLastActivityAsync(user.Id);
        await _authRepository.CommitAsync();

        var userClaims = await _authRepository.GetClaimsAsync(user.Id);
        if (userClaims is null)
        {
            return new(ResultCode.InternalServerError, "Get claims error");
        }

        var accessToken = _jwtFactory.GenerateEncodedToken(userClaims, TokenType.AccessToken);
        var refreshToken = _jwtFactory.GenerateEncodedToken(
            new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id) },
            TokenType.RefreshToken);

        var response = new AccessTokenResponse(accessToken, refreshToken);

        return new(ResultCode.Ok, response);
    }
}