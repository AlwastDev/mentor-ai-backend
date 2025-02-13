using System.Security.Claims;
using Auth.Application.Dto;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Contracts.Persistence;

public interface IAuthRepository : IRepositoryBase<User, UserDto>
{
    Task RegisterAsync(string email, string password);
    Task<IdentityResult> AddClaimsAsync(string userId, IEnumerable<Claim> claims);
    Task<IList<Claim>?> GetClaimsAsync(string userId);
    Task<SignInResult> CheckPasswordSignInAsync(string userId, string password, bool lockoutOnFailure);
    Task SetAccessTokenIssueStateAsync(string userId, bool state);
    Task RevokeAllTokensAsync(string userId);
    Task UpdateLastActivityAsync(string userId);
}