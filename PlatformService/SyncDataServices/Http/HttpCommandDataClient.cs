using System.Text;
using System.Text.Json;
using PlatformService.Dto;
using Microsoft.Extensions.Configuration;

namespace PlatformService.CommandService.SyncDataService.Http
{
    public class HttpCommandDataClient: ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpCommandDataClient> _logger;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, ILogger<HttpCommandDataClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }



        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platform), Encoding.UTF8, "application/json");
            
            var response = await  _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error sending platform to command: {response.StatusCode}");
                throw new Exception("Error sending platform to command");
            }
            _logger.LogInformation("Platform sent to command");
        }
    }
}