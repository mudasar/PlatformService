using PlatformService.Dto;

namespace PlatformService.AsyncDataServices.Http;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishDto);
}