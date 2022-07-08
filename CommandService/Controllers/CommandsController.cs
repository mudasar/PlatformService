using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController : ControllerBase
{
   
    private readonly ILogger<CommandsController> _logger;

    public CommandsController(ILogger<CommandsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCommands")]
    public async Task<IActionResult>  Get()
    {
       return Ok(new { message = "Hello World" });
    }
}
