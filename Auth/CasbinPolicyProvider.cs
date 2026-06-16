using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CasbinTest.Auth;

/// <summary>
/// Turns policy names of the form "obj:act" (e.g. "asset:read") into an
/// AuthorizationPolicy backed by a CasbinRequirement, so endpoints can declare
/// authorization inline with [Authorize("asset:read")] without pre-registering
/// every object/action combination. Anything that isn't in "obj:act" form is
/// handed off to the default provider.
/// </summary>
public class CasbinPolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallback = new(options);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var parts = policyName.Split(':', 2, StringSplitOptions.TrimEntries);

        if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new CasbinRequirement(parts[0], parts[1]))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallback.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();
}
