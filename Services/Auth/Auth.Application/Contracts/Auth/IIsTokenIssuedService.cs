namespace Auth.Application.Contracts.Auth;

public interface IIsTokenIssuedService
{
    Task<bool> IsTokenIssuedAsync(string userId);
}