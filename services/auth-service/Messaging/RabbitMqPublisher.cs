using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace auth_service.Messaging;

public class RabbitMqPublisher : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
            );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {

            Persistent = true,
        };

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body);
    }

    public void Dispose() 
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }

    
}

