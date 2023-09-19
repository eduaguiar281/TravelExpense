using MediatR;

namespace TravelExpense.Core.Communications
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        
        public MediatorHandler(IMediator mediator)
        {
            _mediator= mediator;
        }

        public async Task PublishEventAsync<T>(T domainEvent) where T : DomainEvent
        {
            await _mediator.Publish(domainEvent);
        }

        public async Task<TResult> SendCommandAsync<T, TResult>(T command) where T : Command<TResult>
        {
            return await _mediator.Send(command);
        }

        public async Task<TResult> SendQueryAsync<T, TResult>(T query) where T : Query<TResult>
        {
            return await _mediator.Send(query);
        }

    }
}
