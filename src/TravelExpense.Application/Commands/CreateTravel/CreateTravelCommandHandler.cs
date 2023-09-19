using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Core.ExtensionsMethods;
using TravelExpense.Domain;
using TravelExpense.Domain.ValueObjects;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Commands.CreateTravel
{
    public class CreateTravelCommandHandler : IRequestHandler<CreateTravelCommand, CommandResult<TravelDto>>
    {

        private readonly ITravelRepository _repository;
        public CreateTravelCommandHandler(ITravelRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult<TravelDto>> Handle(CreateTravelCommand request, CancellationToken cancellationToken)
        {
            Result<Employee> employeeResult = Employee.Create(request.EmployeeRegistration, request.EmployeeName);
            var result = Result.Combine(request.Description.FailIfNullOrEmpty("Description cannot be null or empty!"),
                                        request.EmployeeRegistration.FailIfNullOrEmpty("Employee Registration cannot be null or empty!"),
                                        request.EmployeeName.FailIfNullOrEmpty("Employee Name cannot be null or empty!"),
                                        request.EndedIn.FailIfLessThan(request.StartedIn, "Ended In not be less than Started In!"),
                                        employeeResult)
                .Finally(r => r.IsSuccess ? Travel.Create(request.Description, employeeResult.Value, request.StartedIn, request.EndedIn) : Result.Failure<Travel>(r.Error));
            
            if (result.IsFailure)
                return CommandResult<TravelDto>.Failure(result.Error);

            await _repository.CreateTravelAsync(result.Value, cancellationToken);
            return CommandResult<TravelDto>.Success(result.Value);
        }
    }
}
