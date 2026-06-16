using Casbin;
using CasbinTest.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace CasbinTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services
            .AddAuthentication(HeaderAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>(
                HeaderAuthenticationHandler.SchemeName, _ => { });
        // A single shared enforcer, instead of rebuilding model+policy per request.
        builder.Services.AddSingleton<IEnforcer>(
            _ => new Enforcer("Casbin/model.conf", "Casbin/policy.csv"));

        // Casbin-backed authorization: the policy provider turns "obj:act" policy
        // names into CasbinRequirements, and the handler evaluates them.
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, CasbinPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, CasbinAuthorizationHandler>();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
