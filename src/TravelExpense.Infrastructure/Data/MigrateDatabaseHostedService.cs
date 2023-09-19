using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TravelExpense.Infrastructure.Data
{
    public class MigrateDatabaseHostedService: IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MigrateDatabaseHostedService> _logger;

        public MigrateDatabaseHostedService(IServiceProvider serviceProvider, ILogger<MigrateDatabaseHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Migrating database...");
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TravelExpenseDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Migration finished");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
