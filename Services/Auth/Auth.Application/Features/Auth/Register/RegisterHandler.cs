using System.Security.Claims;
using Auth.Application.Contracts.Auth;
using Auth.Application.Contracts.Persistence;
using Auth.Application.Responses;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, ResultResponse<AccessTokenResponse>>
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtFactory _jwtFactory;

    public RegisterHandler(IAuthRepository authRepository, IJwtFactory jwtFactory)
    {
        _authRepository = authRepository;
        _jwtFactory = jwtFactory;
    }

    public async Task<ResultResponse<AccessTokenResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return new(ResultCode.BadRequest, "Email cannot be empty.");
        }

        var user = await _authRepository.GetAsync((u) => u.Email == request.Email);

        if (user is not null)
        {
            return new(ResultCode.NotFound, "Email already taken");
        }

        var createdUser = await _authRepository.AddAsync(new User
        {
            Email = request.Email,
            Discriminator = RoleType.Student.ToString(),
        });
        
        await _authRepository.RegisterAsync(createdUser.Id, request.Password);

        // var registrationToken = _jwtFactory.GenerateEncodedToken(new List<Claim> { new(ClaimTypes.NameIdentifier, expert.Id) }, TokenType.RegistrationToken);
        //
        // if (string.IsNullOrEmpty(registrationToken))
        // {
        //     return new(ResultCode.InternalServerError, "Token generation error");
        // }
        //
        // var emailTemplate = $"{_environmentOptions.Domain}?registrationCode={registrationToken.ToBase64()}";
        //
        // _ = _emailSender.SendEmailAsync(expert.Email, "Registration", emailTemplate);

        var userClaims = await _authRepository.GetClaimsAsync(createdUser.Id);

        if (userClaims is null)
        {
            return new(ResultCode.InternalServerError, "Get claims error");
        }

        if (userClaims.All(static claim => claim.Type != ClaimTypes.Role))
        {
            await _authRepository.AddClaimsAsync(createdUser.Id,
            [
                new Claim(ClaimTypes.NameIdentifier, createdUser.Id),
                new Claim(ClaimTypes.Role, createdUser.UserRole.ToString())
            ]);

            await _authRepository.CommitAsync();
        }

        userClaims = userClaims
            .Where(claim => !(claim.Type == ClaimTypes.Role && claim.Value != createdUser.UserRole.ToString()))
            .ToList();

        var accessToken = _jwtFactory.GenerateEncodedToken(userClaims, TokenType.AccessToken);

        var refreshToken = _jwtFactory.GenerateEncodedToken(
            new List<Claim> { new(ClaimTypes.NameIdentifier, createdUser.Id) },
            TokenType.RefreshToken);

        var response = new AccessTokenResponse(accessToken, refreshToken);

        await _authRepository.SetAccessTokenIssueStateAsync(createdUser.Id, true);

        await _authRepository.UpdateLastActivityAsync(createdUser.Id);

        await _authRepository.CommitAsync();

        return new(ResultCode.Created, response);
    }
}