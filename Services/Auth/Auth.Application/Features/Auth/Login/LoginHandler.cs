using System.Security.Claims;
using Auth.Application.Contracts.Auth;
using Auth.Application.Contracts.Persistence;
using Auth.Application.Responses;
using Auth.Domain.Common;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ResultResponse<AccessTokenResponse>>
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtFactory _jwtFactory;

    public LoginHandler(IAuthRepository authRepository, IJwtFactory jwtFactory)
    {
        _authRepository = authRepository;
        _jwtFactory = jwtFactory;
    }

    public async Task<ResultResponse<AccessTokenResponse>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _authRepository.GetAsync((u) => u.Email == request.Email);

        if (user is null)
        {
            return new(ResultCode.Forbidden, "Your email or password is incorrect");
        }

        var result = await _authRepository.CheckPasswordSignInAsync(user.Id, request.Password, false);

        if (result.IsLockedOut)
        {
            return new(ResultCode.LockedOut, "Your profile is blocked");
        }

        if (!result.Succeeded)
        {
            return new(ResultCode.Forbidden, "Your email or password is incorrect");
        }

        // if (!user.EmailConfirmed)
        // {
        // 	return new(ResultCode.Forbidden, "Your email is not confirmed");
        // }

        var userClaims = await _authRepository.GetClaimsAsync(user.Id);

        if (userClaims is null)
        {
            return new(ResultCode.InternalServerError, "Get claims error");
        }

        if (userClaims.All(static claim => claim.Type != ClaimTypes.Role))
        {
            await _authRepository.AddClaimsAsync(user.Id,
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            ]);

            await _authRepository.CommitAsync();
        }

        userClaims = userClaims
            .Where(claim => !(claim.Type == ClaimTypes.Role && claim.Value != user.UserRole.ToString())).ToList();

        var accessToken = _jwtFactory.GenerateEncodedToken(userClaims, TokenType.AccessToken);

        var refreshToken = _jwtFactory.GenerateEncodedToken(
            new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
            },
            TokenType.RefreshToken);

        var response = new AccessTokenResponse(accessToken, refreshToken);

        await _authRepository.SetAccessTokenIssueStateAsync(user.Id, true);

        await _authRepository.UpdateLastActivityAsync(user.Id);

        await _authRepository.CommitAsync();

        return new(ResultCode.Ok, response);
    }
}