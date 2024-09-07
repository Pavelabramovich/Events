using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace Events.WebApi.Authentication;


public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        IEnumerable<IAuthorizationRequirement> requirements = context.Requirements;

        if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
        {
            context.Fail(new AuthorizationFailureReason(this, "User token has no role"));
            return;
        }

        var role = context.User.FindFirst(x => x.Type == ClaimTypes.Role)!.Value;

        string[] roles = role.Split(',');
        string expectedRole = requirement.Role;

        string[] requireRoles = requirements.Where(y => y.GetType() == typeof(RoleRequirement)).Select(x => ((RoleRequirement)x).Role).ToArray();

        var isMatch = requireRoles.Any(x => roles.Any(y => x == y));

        if (!isMatch)
        {
            context.Fail(new AuthorizationFailureReason(this, "User token doesn't has the required role"));
            return;
        }

        context.Succeed(requirement);
        return;
    }
}
