using TravelExpense.Core;

namespace TravelExpense.Infrastructure.Messages
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IAmqpClient _amqpClient;

        public EventPublisher(IAmqpClient amqpClient)
        {
            _amqpClient = amqpClient;
        }

        public void Publish<T>(T domainEvent) where T : DomainEvent
        {
            ArgumentNullException.ThrowIfNull(nameof(domainEvent));
            _amqpClient.Publish(domainEvent, domainEvent.EventName!);
        }
    }
}
