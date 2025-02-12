using MediatR;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Logout;

public record LogoutCommand(string UserId) : IRequest<ResultResponse>;