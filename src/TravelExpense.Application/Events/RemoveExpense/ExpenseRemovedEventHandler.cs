using MediatR;
using TravelExpense.Domain.Events;
using TravelExpense.Infrastructure.Messages;

namespace TravelExpense.Application.Events.RemoveExpense
{
    public class ExpenseRemovedEventHandler : INotificationHandler<ExpenseRemovedEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        public ExpenseRemovedEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task Handle(ExpenseRemovedEvent notification, CancellationToken cancellationToken)
        {
            _eventPublisher.Publish(notification);
            return Task.CompletedTask;
        }
    }
}
