using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private IServiceScopeFactory _serviceScopeFactory;
    private IMapper _mapper;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            case EventType.Undetermined:
                _logger.LogError("Unable to determine event type");
                return;

        }

    }

    private EventType DetermineEvent(string notificationMessage)
    {
        _logger.LogInformation("Determining event type"); ;

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
        switch (eventType?.Event)
        {
            case "Platform_Published":
                _logger.LogInformation("--> Platform Published event detected");
                return EventType.PlatformPublished;
            default:
                _logger.LogInformation("--> Unknown event detected");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
            var platform = _mapper.Map<Platform>(platformPublishedDto);
            if (repository.ExternalPlatformExists(platform.ExternalID))
            {
                _logger.LogInformation("Platform already exists");
                return;
            }
            repository.AddPlatform(platform);
            repository.SaveChanges();
        }
        catch (Exception ex)
        {
            // TODO
            _logger.LogError($"Error adding platform {ex.Message}", ex);
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}