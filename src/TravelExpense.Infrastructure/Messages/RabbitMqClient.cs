using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TravelExpense.Infrastructure.Messages
{
    public class RabbitMqClient : IAmqpClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(RabbitConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(config.Uri)
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish<T>(T data, string queue)
        {
            ConfigureQueue(queue);
            string message = JsonConvert.SerializeObject(data);
            _channel.BasicPublish("", queue, null, Encoding.UTF8.GetBytes(message));
        }

        public void ListenAll(string queue, Func<string?, bool> handleReceived)
        {
            ArgumentNullException.ThrowIfNull(handleReceived, nameof(handleReceived));
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, @event) =>
            {
                string? message = Encoding.UTF8.GetString(@event.Body.ToArray());
                _channel.BasicAck(@event.DeliveryTag, handleReceived(message));
            };
            _channel.BasicConsume(queue, false, consumer);
        }

        public T? ListenFirst<T>(string queue)
        {
            ConfigureQueue(queue);
            BasicGetResult result = _channel.BasicGet(queue, true);
            if (result is null)
                return default;

            var message = Encoding.UTF8.GetString(result.Body.ToArray());

            return TryConvert<T>(message);
        }

        private static T? TryConvert<T>(string? message)
        {
            if (string.IsNullOrEmpty(message))
                return default;
            try
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                return JsonConvert.DeserializeObject<T>(message, settings);
            }
            catch (Exception ex)
            {
                throw;
                //return default;
            }
        }

        private void ConfigureQueue(string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}
