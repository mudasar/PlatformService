using System.Text;
using System.Text.Json;
using PlatformService.Dto;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices.Http;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private readonly ILogger<MessageBusClient> _logger;
    private readonly IConnection? _connection;
    private readonly IModel? _channel;
    

    public MessageBusClient(ILogger<MessageBusClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        var configuration1 = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = configuration1["RabbitMQHost"],
            Port = int.Parse(configuration1["RabbitMQPort"])
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            _logger.LogInformation("--> RabbitMQ Connection Established");
        }
        catch (Exception ex)
        {
            _logger.LogError($"--> Could not connect to Message Bus {ex.Message}", ex);
        }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation($"--> RabbitMQ Connection Shutdown {e.ReplyText}");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishDto)
    {
        var message = JsonSerializer.Serialize(platformPublishDto);
        if (_connection!.IsOpen)
        {
            _logger.LogInformation("RabbitMQ connection open sending message");
            SendMessage(message);
        }
        else
        {
            _logger.LogInformation("RabbitMQ connection is closed not sending");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
        _logger.LogInformation($" --> We have sent {message}");
    }

    public void Dispose()
    {
        _logger.LogInformation("Message Bus Disposed");
        if (!_channel!.IsOpen) return;
        _channel.Close();
        _connection!.Close();
    }
}