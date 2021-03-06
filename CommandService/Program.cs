using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.EventProcessing;
using CommandService.SyncDataService.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
// TODO: for now using in memory database, but should be replaced with a real database
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("PlatformService"));
// TODO: convert the implementation to pooled factory
builder.Services.AddDbContext<CommandsDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("default")));
//builder.Services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("default")));
// TODO: toggle for sql server
// builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddControllers();

builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Initalize the database with data
// await app.InitializeAsync();

// Run the migrations
using var scope = app.Services.CreateScope();
//var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
await using var dbContext = scope.ServiceProvider.GetRequiredService<CommandsDbContext>();
await dbContext.Database.MigrateAsync();

 await app.InitializeAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
