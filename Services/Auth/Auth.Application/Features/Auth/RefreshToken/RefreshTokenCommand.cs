using Auth.Application.Responses;
using MediatR;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<ResultResponse<AccessTokenResponse>>;