using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Commands.RejectExpense
{
    public class RejectExpenseCommand: Command<CommandResult<TravelDto>>
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; } 
        public string Comment { get; set; } = string.Empty;
    }
}
