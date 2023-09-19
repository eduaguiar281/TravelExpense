using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Core.ExtensionsMethods;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Commands.RejectExpense
{
    public class RejectExpenseCommandHandler : IRequestHandler<RejectExpenseCommand, CommandResult<TravelDto>>
    {
        private readonly ITravelRepository _repository;
        public RejectExpenseCommandHandler(ITravelRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult<TravelDto>> Handle(RejectExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Travel> maybeTravel = await _repository.FindByIdAsync(request.Id, cancellationToken);
            var result = Result.Combine(request.Comment.FailIfEmpty("Comment cannot be null or empty"),
                maybeTravel.HasValue.ShouldBe(true, "Travel not found!"))
                .Finally(r => r.IsSuccess ?
                maybeTravel.Value.Reject(request.ExpenseId, request.Comment) :
                Result.Failure<Travel>(r.Error));

            if (result.IsFailure)
                return CommandResult<TravelDto>.Failure(result.Error);

            await _repository.UpdateAsync(maybeTravel.Value, cancellationToken);
            return CommandResult<TravelDto>.Success(maybeTravel.Value);
        }
    }
}
