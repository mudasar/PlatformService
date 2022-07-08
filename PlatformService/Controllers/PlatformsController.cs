using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.CommandService.SyncDataService.Http;
using PlatformService.Data;
using PlatformService.Dto;
using PlatformService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : Controller
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly ILogger<PlatformsController> _logger;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataClient, ILogger<PlatformsController> logger)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _logger = logger;
    }


    // GET: api/<PlatformsController>
    [HttpGet(Name = "Get AllPlatforms")]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatforms()
    {
        var platforms = await _platformRepository.GetAll();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    // GET api/<PlatformsController>/5
    [HttpGet("{id}", Name = "GetPlatformById")]
    public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
    {
        var platform = await _platformRepository.GetById(id);
        if (platform is null)
        {
            return NotFound(new { error = $"Platform with id '{id}' was not found" });
        }
        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    // POST api/<PlatformsController>
    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> Post([FromBody] PlatformCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var platformModel = _mapper.Map<Platform>(model);
        await _platformRepository.Create(platformModel);
        await _platformRepository.SaveChanges();
        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
             // TODO
            _logger.LogError(ex, "Error sending platform to command service");
        }
        
        return CreatedAtRoute(nameof(GetPlatformById), new {platformReadDto.Id }, platformReadDto);
    }

}
