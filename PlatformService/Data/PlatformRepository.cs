using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _dbContext;

    public PlatformRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<Platform>> GetAll()
    {
        return await _dbContext.Platforms!.ToListAsync();
    }

    public async Task<Platform?> GetById(int id)
    {
        return await _dbContext.Platforms!.FindAsync(id);
    }

    public async Task DeleteById(int id)
    {
        var platform = await _dbContext.Platforms!.FindAsync(id);
        if (platform is null)
        {
            throw new InvalidOperationException("platform with specified id was not found.");
        }
        _dbContext.Platforms.Remove(platform);
    }

    public Task UpdateById(Platform platform)
    {
        _dbContext.Platforms!.Update(platform);
        return Task.CompletedTask;
    }

    public async Task Create(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }
        await _dbContext.Platforms!.AddAsync(platform);
    }
}