using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepository {
    bool SaveChanges();

    IEnumerable<Platform> GetPlatforms();
    Platform GetPlatform(int id);
    bool PlatformExists(int id);
    bool ExternalPlatformExists(int externalId);
    void AddPlatform(Platform platform);
    void RemovePlatform(Platform platform);
    void RemovePlatform(int id);

    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommand(int id);
    Command GetCommand(int platformId, int commandId);
    void AddCommand(int platformId, Command command);
    void RemoveCommand(Command command);
    void RemoveCommand(int id);
    Task SaveChangesAsync();
}