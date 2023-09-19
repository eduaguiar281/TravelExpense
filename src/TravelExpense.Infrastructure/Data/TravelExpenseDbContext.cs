using Microsoft.EntityFrameworkCore;
using TravelExpense.Core;
using TravelExpense.Core.Communications;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Data.EntityConfigurations;

namespace TravelExpense.Infrastructure.Data
{
    public class TravelExpenseDbContext : DbContext
    {

        private readonly IMediatorHandler _mediatorHandler;

        public TravelExpenseDbContext(DbContextOptions<TravelExpenseDbContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Travel> Travels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.ApplyConfiguration(new TravelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseEntityTypeConfiguration());
        }

        public async virtual new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedAtForEntity();
            SetUpdatedAt();
            int rowsAfftected = await base.SaveChangesAsync();
            await PublishEvents();

            return rowsAfftected;
        }

        private async Task PublishEvents()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAggregateRoot aggregateRoot)
                {
                    await SetEventsAndPublish(aggregateRoot);
                }
            }
        }

        private async Task SetEventsAndPublish(IAggregateRoot aggregateRoot)
        {
            foreach (DomainEvent @event in aggregateRoot.Events)
            {
                @event.DomainEntity = aggregateRoot;
                @event.EntityId = aggregateRoot.Id;
                await _mediatorHandler.PublishEventAsync(@event);
            }
        }

        private void SetUpdatedAt()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("UpdatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
            }
        }

        private void SetCreatedAtForEntity()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                }
            }
        }
    }
}
