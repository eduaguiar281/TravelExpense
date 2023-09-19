using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using TravelExpense.Infrastructure.Data;
using TravelExpense.Infrastructure.Messages;

namespace TravelExpense.Integration.Tests
{
    public class TravelExpenseWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder().Build();
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder().Build();
        private IAmqpClient _rabbitMqClient;

        public IAmqpClient AmqpClient { get => _rabbitMqClient; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                ServiceDescriptor? dbContextOptionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TravelExpenseDbContext>));

                ServiceDescriptor? rabbitOptionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(RabbitMqConfiguration));

                services.Remove(dbContextOptionsDescriptor!);
                services.Remove(rabbitOptionsDescriptor!);

                services.AddDbContext<TravelExpenseDbContext>(options =>
                {
                    options.UseSqlServer(GetConnectionString(), sqlServerOptionsAction: actionOptions =>
                    {
                        actionOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(60),
                            errorNumbersToAdd: null);
                    });
                });

                services.AddSingleton(new RabbitConfiguration
                {
                    Uri = _rabbitMqContainer.GetConnectionString()
                });
            });
        }

        public async Task SeedDatabase()
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            string query = File.ReadAllText("SeedDatabase.sql");

            await using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            await using SqlCommand command = connection.CreateCommand();

            command.CommandText = query;
            command.Transaction = transaction;
            await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();
            await _sqlServerContainer.ExecScriptAsync("CREATE DATABASE travel-expense");
            await _rabbitMqContainer.StartAsync();
            _rabbitMqClient = new RabbitMqClient(new RabbitConfiguration {  Uri = _rabbitMqContainer.GetConnectionString() });
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
        }

        private string GetConnectionString()
        {
            return _sqlServerContainer.GetConnectionString().Replace("Database=master", "Database=travel-expense");
        }
    }
}
