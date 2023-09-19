using TravelExpense.Domain.Enums;
using TravelExpense.Domain.ValueObjects;
using TravelExpense.Domain;

namespace TravelExpense.Application.Dtos
{
    public record TravelDto (long Id,
        string Description,
        EmployeeDto Employee,
        DateTime StartedIn,
        DateTime EndedIn,
        TravelStatus Status,
        decimal TotalExpenses,
        List<ExpenseDto> Expenses,
        DateTime CreatedAt, 
        DateTime? UpdatedAt)
    {
        public static implicit operator TravelDto(Travel travel) =>
            new(travel.Id,
                 travel.Description,
                 travel.Employee,
                 travel.StartedIn,
                 travel.EndedIn,
                 travel.Status,
                 travel.TotalExpenses,
                 travel.Expenses.ToList().Select(e => (ExpenseDto)e).ToList(),
                 travel.CreatedAt,
                 travel.UpdatedAt);

    }
}
