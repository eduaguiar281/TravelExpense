using CSharpFunctionalExtensions;

namespace TravelExpense.Core
{
    public class DomainEntity: Entity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
