using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
   
    private readonly ILogger<PlatformsController> _logger;
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    public PlatformsController(ILogger<PlatformsController> logger, ICommandRepository commandRepository, IMapper mapper)
    {
        _logger = logger;
        _commandRepository = commandRepository;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetPlatforms")]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>>  GetPlatforms()
    {
        _logger.LogInformation("GetPlatforms was called");
        var platforms = _commandRepository.GetPlatforms();
        await Task.Delay(0);
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }
}
