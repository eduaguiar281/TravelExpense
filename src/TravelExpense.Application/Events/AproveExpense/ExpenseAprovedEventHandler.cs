using MediatR;
using TravelExpense.Domain.Events;
using TravelExpense.Infrastructure.Messages;

namespace TravelExpense.Application.Events.AproveExpense
{
    public class ExpenseAprovedEventHandler : INotificationHandler<ExpenseAprovedEvent>
    {
        private readonly IEventPublisher _eventPublisher;
        public ExpenseAprovedEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task Handle(ExpenseAprovedEvent notification, CancellationToken cancellationToken)
        {
            _eventPublisher.Publish(notification);
            return Task.CompletedTask;
        }
    }
}
