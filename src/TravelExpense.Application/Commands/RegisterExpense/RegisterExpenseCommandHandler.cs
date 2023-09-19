using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Core.ExtensionsMethods;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Commands.RegisterExpense
{
    public class RegisterExpenseCommandHandler : IRequestHandler<RegisterExpenseCommand, CommandResult<TravelDto>>
    {
        private readonly ITravelRepository _travelRepository;

        public RegisterExpenseCommandHandler(ITravelRepository travelRepository)
        {
            _travelRepository = travelRepository;
        }

        public async Task<CommandResult<TravelDto>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {

            Maybe<Travel> maybeTravel = await _travelRepository.FindByIdAsync(request.Id, cancellationToken);
            var result = Result.Combine(request.RelatedTo.FailIfEmpty("Related To cannot be null or empty"),
                request.Description.FailIfNullOrEmpty("Description cannot be null or empty!"),
                request.Value.FailIfLessThanOrEquals(0, "Value cannot less than or equals zero!"),
                maybeTravel.HasValue.ShouldBe(true, "Travel not found!"))
                .Finally(r => r.IsSuccess ? 
                maybeTravel.Value.RegisterExpense(request.RelatedTo, request.Description, request.Value, request.Date) : 
                Result.Failure<Travel>(r.Error));

            if (result.IsFailure)
                return CommandResult<TravelDto>.Failure(result.Error);

            await _travelRepository.UpdateAsync(maybeTravel.Value, cancellationToken);
            return CommandResult<TravelDto>.Success(maybeTravel.Value);
        }
    }
}
