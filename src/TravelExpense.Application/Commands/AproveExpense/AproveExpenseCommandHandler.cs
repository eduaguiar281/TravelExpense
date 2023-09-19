using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Core.ExtensionsMethods;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Commands.AproveExpense
{
    public class AproveExpenseCommandHandler : IRequestHandler<AproveExpenseCommand, CommandResult<TravelDto>>
    {
        private readonly ITravelRepository _repository;
        public AproveExpenseCommandHandler(ITravelRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult<TravelDto>> Handle(AproveExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Travel> maybeTravel = await _repository.FindByIdAsync(request.Id, cancellationToken);
            var result = Result.Combine(request.VoucherId.FailIfEmpty("VoucherId cannot be null or empty"),
                maybeTravel.HasValue.ShouldBe(true, "Travel not found!"))
                .Finally(r => r.IsSuccess ?
                maybeTravel.Value.Aprove(request.ExpenseId, request.VoucherId, request.Comment) :
                Result.Failure<Travel>(r.Error));

            if (result.IsFailure)
                return CommandResult<TravelDto>.Failure(result.Error);

            await _repository.UpdateAsync(maybeTravel.Value, cancellationToken);
            return CommandResult<TravelDto>.Success(maybeTravel.Value);
        }
    }
}
