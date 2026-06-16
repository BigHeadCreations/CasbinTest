using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace CasbinTest.Auth;

/// <summary>
/// A minimal authentication handler for local testing. It reads the user name
/// from the "X-User" request header and builds a ClaimsPrincipal from it, so you
/// can set the current user from curl without wiring up a real identity provider.
///
/// Example: curl -H "X-User: alice" https://localhost:xxxx/assets/1
/// </summary>
public class HeaderAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Header";
    public const string HeaderName = "X-User";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderName, out var userName) ||
            string.IsNullOrWhiteSpace(userName))
        {
            // No header supplied: let the request through as anonymous.
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new[] { new Claim(ClaimTypes.Name, userName.ToString()) };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
