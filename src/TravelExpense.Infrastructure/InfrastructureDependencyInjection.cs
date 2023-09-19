using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelExpense.Infrastructure.Data;
using TravelExpense.Infrastructure.Messages;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationRoot configuration)
        {
            string? connectionString = configuration.GetConnectionString("TravelConnection");

            services.AddDbContext<TravelExpenseDbContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptionsAction: actionOptions =>
                {
                    actionOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(60),
                        errorNumbersToAdd: null);
                }));
            services.AddHostedService<MigrateDatabaseHostedService>();
            services.AddSingleton<RabbitConfiguration>(new RabbitConfiguration 
                { 
                    Uri = configuration.GetConnectionString("RabbitMq") ?? "" 
                });


            services.AddScoped<ITravelRepository, TravelRepository>();
            services.AddScoped<IAmqpClient, RabbitMqClient>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            return services;
        }
    }
}
