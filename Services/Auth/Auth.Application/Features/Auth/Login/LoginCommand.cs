using Auth.Application.Responses;
using MediatR;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<ResultResponse<AccessTokenResponse>>;
