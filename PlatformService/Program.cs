using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices.Http;
using PlatformService.CommandService.SyncDataService.Http;
using PlatformService.Data;
using PlatformService.SyncDataService.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
// TODO: for now using in memory database, but should be replaced with a real database
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("PlatformService"));
// TODO: convert the implementation to pooled factory
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("default")));
//builder.Services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("default")));
// TODO: toggle for sql server
// builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc(); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initalize the database with data
// await app.InitializeAsync();

// Run the migrations
using var scope = app.Services.CreateScope();
//var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcPlatformService>();
    endpoints.MapGet("/protos/platforms.proto", async context =>
    {
        await context.Response.WriteAsync(
            File.ReadAllText("PlatformService/SyncDataServices/Grpc/platforms.proto"));
    });
});

app.Run();
