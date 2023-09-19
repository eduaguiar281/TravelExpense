using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Data;

namespace TravelExpense.Infrastructure.Repositories
{
    public class TravelRepository: ITravelRepository
    {
        private readonly TravelExpenseDbContext _dbContext;

        public TravelRepository(TravelExpenseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateTravelAsync(Travel travel, CancellationToken cancellationToken = default)
        {
            await _dbContext.Travels.AddAsync(travel, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Maybe<Travel>> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Travels
                .Include(t => t.Expenses)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Travel travel, CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
