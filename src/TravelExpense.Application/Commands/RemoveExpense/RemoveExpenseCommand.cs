using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Commands.RemoveExpense
{
    public class RemoveExpenseCommand : Command<CommandResult<TravelDto>>
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
    }
}
