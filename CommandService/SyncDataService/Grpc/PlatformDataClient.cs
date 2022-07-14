using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using  PlatformService;

namespace CommandService.SyncDataService.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly  IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly   ILogger<PlatformDataClient> _logger;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper, ILogger<PlatformDataClient> logger)
    {
        _configuration = configuration;
        _mapper = mapper;
        _logger = logger;
    }
    
    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        _logger.LogInformation($"Calling Grpc Server {_configuration["GrpcPlatform"]}");

        var channel =  GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (System.Exception ex)
        {
             _logger.LogError($"Error calling Grpc Server {_configuration["GrpcPlatform"]} , error : {ex.Message}", ex);
            return null;
        }
    }
}
