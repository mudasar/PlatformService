using AutoMapper;
using CommandService.Dto;
using CommandService.Models;

namespace CommandService.Profiles;

public class CommandProfile : Profile {
    public CommandProfile() {
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Platform, PlatformReadDto>();
    }
}