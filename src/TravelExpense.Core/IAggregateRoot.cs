namespace TravelExpense.Core
{
    public interface IAggregateRoot
    {
        long Id { get; }
        IReadOnlyCollection<DomainEvent> Events { get; } 
    }
}
