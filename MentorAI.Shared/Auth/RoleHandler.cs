using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MentorAI.Shared.Auth;

public class RoleHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var requirementRoles = requirement.Roles.Split(',');
        var roles = context.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value);

        if (roles.Intersect(requirementRoles).Any())
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.FromResult(0);
    }
}