using Auth.Application.Contracts.Persistence;
using Auth.Application.Dto;
using Auth.Infrastructure.Persistence;
using Auth.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Claim = System.Security.Claims.Claim;

namespace Auth.Infrastructure.Repositories;

public class AuthRepository : RepositoryBase<User, UserDto>, IAuthRepository
{
    private readonly AuthContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthRepository(
        AuthContext context,
        IMapper mapper,
        UserManager<User> userManager,
        SignInManager<User> signInManager) : base(context, mapper)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<IdentityResult> AddClaimsAsync(string userId, IEnumerable<Claim> claims)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        return await _userManager.AddClaimsAsync(user, claims);
    }
    
    public async Task<IList<Claim>?> GetClaimsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return await _signInManager.UserManager.GetClaimsAsync(user);
    }

    public async Task<SignInResult> CheckPasswordSignInAsync(string userId, string password, bool lockoutOnFailure)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return SignInResult.Failed;
        }

        return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
    }

    public async Task SetAccessTokenIssueStateAsync(string userId, bool state)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is not null)
        {
            user.IsAuthTokenIssued = state;
        }
    }
    
    public async Task UpdateLastActivityAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user is not null)
        {
            user.LastActivityDate = DateTime.UtcNow;
        }
    }
}