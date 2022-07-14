using CommandService.Models;
using System.Linq;

namespace CommandService.Data;

public class CommandRepository : ICommandRepository {

    private readonly CommandsDbContext _context;
    public CommandRepository(CommandsDbContext context)
    {
        _context = context;
    }

    public void AddCommand(int platformId, Command command)
    {
        if (command is null)
        {
            throw new ArgumentNullException("command");
        }
        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }

    public void AddPlatform(Platform platform)
    {
        if (platform is null)
        {
            throw new System.ArgumentNullException(nameof(platform));
        }
        _context.Platforms.Add(platform);
    }

    public Command GetCommand(int id)
    {
        return _context.Commands?.Find(id);
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return _context.Commands?.FirstOrDefault(x => x.PlatformId == platformId && x.Id == commandId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _context.Commands?.Where(x => x.PlatformId == platformId);
    }

    public Platform GetPlatform(int id)
    {
        return _context.Platforms?.Find(id);
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        return _context.Platforms?.ToList();
    }

    public bool PlatformExists(int id)
    {
        return _context.Platforms?.Any(x => x.Id == id) ?? false;
    }

    public void RemoveCommand(Command command)
    {
        _context.Commands?.Remove(command);
    }

    public void RemoveCommand(int id)
    {
        _context.Commands?.Remove(GetCommand(id));
    }

    public void RemovePlatform(Platform platform)
    {
        _context.Platforms?.Remove(platform);
    }

    public void RemovePlatform(int id)
    {
        _context.Platforms?.Remove(_context.Platforms?.Find(id));
    }

    public bool SaveChanges()
    {
        _context.SaveChanges();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}