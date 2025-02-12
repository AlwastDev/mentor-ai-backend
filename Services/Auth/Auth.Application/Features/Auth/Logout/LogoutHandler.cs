using Auth.Application.Contracts.Persistence;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, ResultResponse>
{
    private readonly IAuthRepository _authRepository;

    public LogoutHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<ResultResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authRepository.SetAccessTokenIssueStateAsync(request.UserId, false);

        await _authRepository.CommitAsync();

        return new(ResultCode.Ok);
    }
}