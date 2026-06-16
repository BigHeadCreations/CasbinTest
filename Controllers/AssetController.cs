using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasbinTest.Controllers;

[ApiController]
[Route("assets")]
public class AssetController(ILogger<AssetController> logger) : ControllerBase
{
    private readonly ILogger<AssetController> _logger = logger;

    // The "asset:read" policy is evaluated by Casbin against the current user.
    [Authorize("asset:read")]
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        _logger.LogInformation("Read asset {Id} by {User}", id, User.Identity?.Name);
        return Ok(id);
    }

    [Authorize("asset:write")]
    [HttpPost]
    public ActionResult Create()
    {
        _logger.LogInformation("Create asset by {User}", User.Identity?.Name);
        return Ok();
    }

    [Authorize("asset:delete")]
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _logger.LogInformation("Delete asset {Id} by {User}", id, User.Identity?.Name);
        return NoContent();
    }
}
