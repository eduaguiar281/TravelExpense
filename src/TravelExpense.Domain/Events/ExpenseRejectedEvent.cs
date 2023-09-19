using TravelExpense.Core;

namespace TravelExpense.Domain.Events
{
    public class ExpenseRejectedEvent: DomainEvent
    {
        public override string? EventName
        {
            get => nameof(ExpenseRejectedEvent);
        }
        public override string? EntityName { get => nameof(Travel); }
        public required long ExpenseId { get; set; } 
        public required string Comment { get; set; }
    }
}
