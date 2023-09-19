using MediatR;
using TravelExpense.Domain.Events;
using TravelExpense.Infrastructure.Messages;

namespace TravelExpense.Application.Events.CreateTravel
{
    public class TravelCreatedEventHandler : INotificationHandler<TravelCreatedEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        public TravelCreatedEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task Handle(TravelCreatedEvent notification, CancellationToken cancellationToken)
        {
            _eventPublisher.Publish(notification);
            return Task.CompletedTask;
        }
    }
}
