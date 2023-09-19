using TravelExpense.Core;

namespace TravelExpense.Infrastructure.Messages
{
    public interface IEventPublisher
    {
        void Publish<T>(T domainEvent) where T:DomainEvent;
    }
}
