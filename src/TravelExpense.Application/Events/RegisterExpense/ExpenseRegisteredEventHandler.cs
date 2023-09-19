using MediatR;
using TravelExpense.Domain.Events;
using TravelExpense.Infrastructure.Messages;

namespace TravelExpense.Application.Events.RegisterExpense
{
    public class ExpenseRegisteredEventHandler : INotificationHandler<ExpenseRegisteredEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        public ExpenseRegisteredEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task Handle(ExpenseRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _eventPublisher.Publish(notification);
            return Task.CompletedTask;
        }
    }
}
