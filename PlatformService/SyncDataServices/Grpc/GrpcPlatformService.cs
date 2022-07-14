

using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataService.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext  context)
    {
        var response = new PlatformResponse();
        var platform = await _platformRepository.GetAll();
        foreach (var item in platform)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(item));
        }
        return response;
    }
}