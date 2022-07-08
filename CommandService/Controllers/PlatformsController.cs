using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
   
    private readonly ILogger<PlatformsController> _logger;

    public PlatformsController(ILogger<PlatformsController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "GetPlatforms")]
    public async Task<IActionResult>  GetPlatforms()
    {
        _logger.LogInformation("GetPlatforms was called");

        return Ok(new { message = "Hello World" });
    }

    [HttpGet("test-inbound-connections", Name = "TestInboundConnections")]
    public async Task<IActionResult>  TestInboundConnections()
    {
        _logger.LogInformation("TestInboundConnections was called");
       return Ok(new { message = "Inbound Test Ok" });
    }
}
