
using MediatR;

namespace TravelExpense.Core
{

    public abstract class DomainEvent: INotification 
    {
        public virtual string? EntityName { get; protected set; }
        public virtual long EntityId { get; set; }
        public virtual string? EventName { get; protected set; }
        public virtual object? DomainEntity { get; set; }
    }
}
