using Microsoft.AspNetCore.Authorization;

namespace CasbinTest.Auth;

/// <summary>
/// An authorization requirement that carries the Casbin object (resource) and
/// action for a given endpoint. The subject is taken from the current user at
/// evaluation time. Produced from "[Authorize("obj:act")]" by CasbinPolicyProvider.
/// </summary>
public class CasbinRequirement(string obj, string act) : IAuthorizationRequirement
{
    public string Obj { get; } = obj;
    public string Act { get; } = act;
}
