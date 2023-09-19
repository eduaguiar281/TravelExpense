using TravelExpense.Domain;
using TravelExpense.Domain.Enums;
using TravelExpense.Domain.ValueObjects;

namespace TravelExpense.Application.Dtos
{
    public record ExpenseDto (
        long Id, 
        string RelatedTo, 
        string Description, 
        string? VoucherId, 
        decimal Value, 
        DateTime Date,
        ExpenseStatus Status,
        string? Comments,
        DateTime CreatedAt,
        DateTime? UpdatedAt)
    {

        public static implicit operator ExpenseDto(Expense expense) => 
            new(expense.Id, 
                expense.RelatedTo, 
                expense.Description, 
                expense.VoucherId, 
                expense.Value, 
                expense.Date, 
                expense.Status, 
                expense.Comments, 
                expense.CreatedAt, 
                expense.UpdatedAt);

    }
}
