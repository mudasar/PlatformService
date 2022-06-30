using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    Task<bool> SaveChanges();

    Task<IEnumerable<Platform>> GetAll();
    Task<Platform?> GetById(int id);

    Task DeleteById(int id);
    Task UpdateById(Platform platform);
    Task Create(Platform platform);
}