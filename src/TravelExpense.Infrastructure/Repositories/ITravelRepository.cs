using CSharpFunctionalExtensions;
using TravelExpense.Core;
using TravelExpense.Domain;

namespace TravelExpense.Infrastructure.Repositories
{
    public interface ITravelRepository 
    {
        Task CreateTravelAsync(Travel travel, CancellationToken cancellationToken = default);

        Task<Maybe<Travel>> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task UpdateAsync(Travel travel, CancellationToken cancellationToken = default);
    }
}
