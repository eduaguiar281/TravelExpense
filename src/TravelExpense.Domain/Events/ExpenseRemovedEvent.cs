using TravelExpense.Core;

namespace TravelExpense.Domain.Events
{
    public class ExpenseRemovedEvent : DomainEvent
    {
        public override string? EventName
        {
            get => nameof(ExpenseRemovedEvent);
        }

        public long ExpenseId { get; set; }
        public override string? EntityName { get => nameof(Travel); }

    }
}
