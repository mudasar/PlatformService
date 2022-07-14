using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId}/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    private readonly ILogger<CommandsController> _logger;

    public CommandsController(ILogger<CommandsController> logger, ICommandRepository commandRepository, IMapper mapper)
    {
        _logger = logger;
        _commandRepository = commandRepository;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetCommandsForPlatform")]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatform([FromRoute] int platformId)
    {
        _logger.LogInformation("GetCommandsForPlatform was called with {platformId}", platformId);
        if (_commandRepository.PlatformExists(platformId))
        {
            var commands = _commandRepository.GetCommandsForPlatform(platformId);
            await Task.Delay(0);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }
        else
        {
            _logger.LogInformation("GetCommandsForPlatform was called with {platformId} but platform does not exist", platformId);
            return NotFound();
        }
    }

    [HttpGet("{id}", Name = "GetCommandForPlatformById")]
    public async Task<ActionResult<CommandReadDto>> GetCommandForPlatformById([FromRoute] int platformId, [FromRoute] int id)
    {
        if (_commandRepository.PlatformExists(platformId))
        {
            var command = _commandRepository.GetCommand(id);
            if (command is null)
            {
                return NotFound(new { error = $"Command with id '{id}' was not found" });
            }
            await Task.Delay(0);

            return Ok(_mapper.Map<CommandReadDto>(command));
        }
        else
        {
            _logger.LogInformation("GetCommandForPlatformById was called with {platformId} but platform does not exist", platformId);
            return NotFound();
        }
    }

    [HttpPost(Name = "CreateCommandForPlatform")]
    public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform([FromRoute] int platformId, [FromBody] CommandCreateDto commandCreateDto)
    {
        if (_commandRepository.PlatformExists(platformId))
        {
            var command = _mapper.Map<Command>(commandCreateDto);
            _commandRepository.AddCommand(platformId, command);
            await _commandRepository.SaveChangesAsync();
            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute("GetCommandForPlatformById", new { platformId, id = commandReadDto.Id }, commandReadDto);
        }
        else
        {
            _logger.LogInformation("CreateCommandForPlatform was called with {platformId} but platform does not exist", platformId);
            return NotFound();
        }
    }

    [HttpDelete("{id}", Name = "DeleteCommandForPlatformById")]
    public async Task<ActionResult> DeleteCommandForPlatformById([FromRoute] int platformId, [FromRoute] int id)
    {
        if (_commandRepository.PlatformExists(platformId))
        {
            var command = _commandRepository.GetCommand(id);
            if (command is null)
            {
                return NotFound(new { error = $"Command with id '{id}' was not found" });
            }
            _commandRepository.RemoveCommand(command);
            await _commandRepository.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            _logger.LogInformation("DeleteCommandForPlatformById was called with {platformId} but platform does not exist", platformId);
            return NotFound();
        }
    }
}
