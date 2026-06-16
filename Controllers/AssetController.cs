using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasbinTest.Controllers;

[AllowAnonymous]
[ApiController]
public class AssetController(ILogger<AssetController> logger) : ControllerBase
{
    private readonly ILogger<AssetController> _logger = logger;

    [HttpGet("details/{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        return StatusCode(200);
    }

}
