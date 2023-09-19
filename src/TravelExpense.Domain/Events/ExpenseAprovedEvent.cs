using TravelExpense.Core;

namespace TravelExpense.Domain.Events
{
    public class ExpenseAprovedEvent: DomainEvent
    {
        public override string? EventName
        {
            get => nameof(ExpenseAprovedEvent);
        }
        public override string? EntityName { get => nameof(Travel); }
        public required long ExpenseId { get; set; }
        public required string VoucherId { get; set; }
        public required string Comment { get; set; }
    }
}
