using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>().HasData(new List<Platform>()
        {
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
    }

    public DbSet<Platform>? Platforms { get; set; }
}

