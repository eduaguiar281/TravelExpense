using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Queries.GetExpenseById
{
    public class GetExpenseByIdQuery : Query<QueryResult<ExpenseDto>>
    {
        public long Id { get; set; }
        public long TravelId { get; set; }
    }
}
