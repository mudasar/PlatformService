using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber: BackgroundService
{
    private IConfiguration _configuration;
    private ILogger<MessageBusSubscriber> _logger;
    private  readonly IEventProcessor _eventProcessor;
    private  IConnection _connection;
    private  IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration configuration,
                                ILogger<MessageBusSubscriber> logger,
                                IEventProcessor evenProcessor)
    {
        _configuration = configuration;
        _logger = logger;
        _eventProcessor = evenProcessor;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq(){
        var connectionString = _configuration.GetConnectionString("RabbitMqConnection");
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"],  Port = int.Parse(_configuration["RabbitMQPort"])};
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queueName =  _channel.QueueDeclare().QueueName;

        _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
        _logger.LogInformation("RabbitMQ connection initialized");
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation($"RabbitMQ connection shutdown {e.ReplyText}");
    }

    public  override void  Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();            
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += Consumer_Received;
        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
    {
        _logger.LogInformation($"Received message {e.Body}");
        var body = e.Body;
        var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
        _eventProcessor.ProcessEvent(notificationMessage);
    }
}
