using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Commands.AproveExpense
{
    public class AproveExpenseCommand : Command<CommandResult<TravelDto>>
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public string VoucherId { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
