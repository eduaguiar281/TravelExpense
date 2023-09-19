using TravelExpense.Core;

namespace TravelExpense.Domain.Events
{
    public class ExpenseRegisteredEvent: DomainEvent
    {
        public override string? EventName
        {
            get => nameof(ExpenseRegisteredEvent);
        }
        public required string RelatedTo { get; set; }
        public required string Description { get; set; } 
        public required decimal Value { get; set; } 
        public required DateTime Date { get; set; }
        public override string? EntityName { get => nameof(Travel); }
    }
}
