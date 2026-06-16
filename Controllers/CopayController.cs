using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasbinTest.Controllers;

[ApiController]
[Route("copays")]
public class CopayController(ILogger<CopayController> logger) : ControllerBase
{
    private readonly ILogger<CopayController> _logger = logger;

    // The "copay:read" policy is evaluated by Casbin against the current user.
    [Authorize("copay:read")]
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        _logger.LogInformation("Read copay {Id} by {User}", id, User.Identity?.Name);
        return Ok(id);
    }

    [Authorize("copay:write")]
    [HttpPost]
    public ActionResult Create()
    {
        _logger.LogInformation("Create copay by {User}", User.Identity?.Name);
        return Ok();
    }

    [Authorize("copay:delete")]
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _logger.LogInformation("Delete copay {Id} by {User}", id, User.Identity?.Name);
        return NoContent();
    }
}
