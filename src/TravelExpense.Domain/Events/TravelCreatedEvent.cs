using TravelExpense.Core;
using TravelExpense.Domain.ValueObjects;

namespace TravelExpense.Domain.Events
{
    public class TravelCreatedEvent: DomainEvent
    {
        public override string? EventName
        {
            get => nameof(TravelCreatedEvent);
        }

        public required Employee Employee { get; set; }
        public required string Description { get; set; }
        public required DateTime StartedIn { get; set; }  
        public required DateTime EndedIn { get; set; }
        public override string? EntityName { get => nameof(Travel); }
    }
}
