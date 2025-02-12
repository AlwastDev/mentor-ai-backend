using Microsoft.AspNetCore.Authorization;

namespace Auth.Infrastructure.Auth;

public class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(string roles)
    {
        Roles = roles;
    }

    public string Roles { get; }
}