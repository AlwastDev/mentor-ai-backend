using Auth.Application.Contracts.Auth;
using Auth.Application.Contracts.Persistence;

namespace Auth.Infrastructure.Services;

public class IsTokenIssuedService : IIsTokenIssuedService
{
    private readonly IAuthRepository _authRepository;

    public IsTokenIssuedService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<bool> IsTokenIssuedAsync(string userId)
    {
        var user = await _authRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        var result = user.IsAuthTokenIssued;

        if (result)
        {
            await _authRepository.UpdateLastActivityAsync(userId);

            await _authRepository.CommitAsync();
        }

        return result;
    }
}