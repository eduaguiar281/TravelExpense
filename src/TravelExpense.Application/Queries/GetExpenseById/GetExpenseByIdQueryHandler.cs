using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Queries.GetExpenseById
{
    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, QueryResult<ExpenseDto>>
    {
        private readonly ITravelRepository _travelRepository;

        public GetExpenseByIdQueryHandler(ITravelRepository travelRepository)
        {
            _travelRepository = travelRepository;
        }

        public async Task<QueryResult<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            Maybe<Travel> maybeTravel = await _travelRepository.FindByIdAsync(request.TravelId, cancellationToken);

            if (!maybeTravel.HasValue)
                return QueryResult<ExpenseDto>.Failure("Travel not found!");

            Maybe<Expense> maybeExpense = maybeTravel.Value.Expenses.FirstOrDefault(e => e.Id == request.Id);
            
            if (!maybeExpense.HasValue)
                return QueryResult<ExpenseDto>.Failure("Expense not found!");

            return QueryResult<ExpenseDto>.Success(maybeExpense.Value);
        }
    }
}
