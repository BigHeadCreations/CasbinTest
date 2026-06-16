using Casbin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasbinTest.Controllers;

[AllowAnonymous]
[ApiController]
[Route("assets")]
public class AssetController(ILogger<AssetController> logger) : ControllerBase
{
    private readonly ILogger<AssetController> _logger = logger;

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var sub = "alice"; // the user that wants to access a resource.
        var obj = "data"; // the resource that is going to be accessed.
        var act = "read"; // the operation that the user performs on the resource.

        var e = new Enforcer("Casbin/model.conf", "Casbin/policy.csv");
        var isEnabled = e.Enabled;
        Console.WriteLine($"Casbin Enabled: {isEnabled}");

        if (e.Enforce(sub, obj, act))
        {
            return Ok(id);
        }
        
        return NotFound();
        
        
    }

}
