using Auth.Application.Responses;
using MediatR;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Register;

public record RegisterCommand(string Email, string Password) : IRequest<ResultResponse<AccessTokenResponse>>;