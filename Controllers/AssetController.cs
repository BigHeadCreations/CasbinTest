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
        return Ok(id);
    }

}
