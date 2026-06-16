using Casbin;
using Microsoft.AspNetCore.Authorization;

namespace CasbinTest.Auth;

/// <summary>
/// Bridges ASP.NET Core authorization to Casbin. For each CasbinRequirement it
/// takes the subject from the authenticated user and asks the shared enforcer
/// whether (sub, obj, act) is allowed.
/// </summary>
public class CasbinAuthorizationHandler(
    IEnforcer enforcer,
    ILogger<CasbinAuthorizationHandler> logger) : AuthorizationHandler<CasbinRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, CasbinRequirement requirement)
    {
        var sub = context.User.Identity?.Name;

        if (string.IsNullOrEmpty(sub))
        {
            // No authenticated subject: leave the requirement unmet -> 401/403.
            logger.LogDebug("Casbin: no subject on request, denying {Obj}:{Act}",
                requirement.Obj, requirement.Act);
            return Task.CompletedTask;
        }

        if (enforcer.Enforce(sub, requirement.Obj, requirement.Act))
        {
            logger.LogDebug("Casbin: allow {Sub} -> {Obj}:{Act}",
                sub, requirement.Obj, requirement.Act);
            context.Succeed(requirement);
        }
        else
        {
            logger.LogDebug("Casbin: deny {Sub} -> {Obj}:{Act}",
                sub, requirement.Obj, requirement.Act);
        }

        return Task.CompletedTask;
    }
}
