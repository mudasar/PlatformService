using PlatformService.Models;

namespace PlatformService.Data;

public static class InMemoryDataGenerator{
    public static async Task InitializeAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        if (context.Platforms!.Any())
        {
            return;
        }
        await context.Platforms!.AddRangeAsync(entities: new Platform[] {
            new Platform
            {
                Id = 1,
                Name = "Dot net",
                Publisher = "Microsoft",
                Cost = "Free"
            },            new Platform
            {
                Id = 2,
                Name = "Kubernetes",
                Publisher = "Cloud Native Computing Foundation",
                Cost = "Free"
            },            new Platform
            {
                Id = 3,
                Name = "Sql Server",
                Publisher = "Microsoft",
                Cost = "Free"
            },            new Platform
            {
                Id = 4,
                Name = "Lambda",
                Publisher = "Amazon",
                Cost = "Free"
            },            new Platform
            {
                Id = 5,
                Name = "Firebase",
                Publisher = "Google",
                Cost = "Free"
            },
        });
        await context.SaveChangesAsync();
        return;   // Data was already seeded
    }
}