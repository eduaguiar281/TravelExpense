using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Core.ExtensionsMethods;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Commands.RemoveExpense
{
    public class RemoveExpenseCommandHandler : IRequestHandler<RemoveExpenseCommand, CommandResult<TravelDto>>
    {
        private readonly ITravelRepository _travelRepository;

        public RemoveExpenseCommandHandler(ITravelRepository travelRepository)
        {
            _travelRepository = travelRepository;
        }

        public async Task<CommandResult<TravelDto>> Handle(RemoveExpenseCommand request, CancellationToken cancellationToken)
        {
            Maybe<Travel> maybeTravel = await _travelRepository.FindByIdAsync(request.Id, cancellationToken);
            var result = maybeTravel.HasValue.ShouldBe(true, "Travel not found!")
                .Finally(r => r.IsSuccess ? maybeTravel.Value.RemoveExpense(request.ExpenseId) : Result.Failure<Travel>(r.Error));

            if (result.IsFailure)
                return CommandResult<TravelDto>.Failure(result.Error);

            await _travelRepository.UpdateAsync(maybeTravel.Value, cancellationToken);
            return CommandResult<TravelDto>.Success(maybeTravel.Value);

        }
    }
}
