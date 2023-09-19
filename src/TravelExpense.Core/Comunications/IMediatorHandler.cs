
namespace TravelExpense.Core.Communications
{
    public interface IMediatorHandler
    {
        Task<TResult> SendCommandAsync<T, TResult>(T command) where T:Command<TResult>;

        Task PublishEventAsync<T>(T domainEvent) where T : DomainEvent;


        Task<TResult> SendQueryAsync<T, TResult>(T query) where T : Query<TResult>;
    }
}
