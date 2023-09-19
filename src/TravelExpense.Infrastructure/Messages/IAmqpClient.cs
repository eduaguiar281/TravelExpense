using RabbitMQ.Client.Events;

namespace TravelExpense.Infrastructure.Messages
{
    public interface IAmqpClient: IDisposable
    {
        void ListenAll(string queue, Func<string?, bool> handleReceived);
        T? ListenFirst<T>(string queue);
        void Publish<T>(T data, string queue);
    }
}