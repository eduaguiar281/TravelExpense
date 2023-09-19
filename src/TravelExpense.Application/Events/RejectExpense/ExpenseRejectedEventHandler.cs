using MediatR;
using TravelExpense.Domain.Events;
using TravelExpense.Infrastructure.Messages;

namespace TravelExpense.Application.Events.RejectExpense
{
    public class ExpenseRejectedEventHandler : INotificationHandler<ExpenseRejectedEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        public ExpenseRejectedEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task Handle(ExpenseRejectedEvent notification, CancellationToken cancellationToken)
        {
            _eventPublisher.Publish(notification);
            return Task.CompletedTask;
        }
    }
}
