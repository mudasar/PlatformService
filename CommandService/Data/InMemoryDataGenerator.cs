
using CommandService.Models;
using CommandService.SyncDataService.Grpc;

namespace CommandService.Data;

public static class GrpcPlatformDataService
{
    public static async Task InitializeAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CommandsDbContext>();
        context.Database.EnsureCreated();
        if (context.Platforms!.Any())
        {
            return;
        }

        var platformDataClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();

        var platforms = platformDataClient.ReturnAllPlatforms();
        await context.Platforms!.AddRangeAsync(platforms);
        await context.SaveChangesAsync();
        return;   // Data was already seeded
    }
}