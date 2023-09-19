using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Commands.RegisterExpense
{
    public class RegisterExpenseCommand : Command<CommandResult<TravelDto>>
    {
        public long Id { get; set; }
        public string RelatedTo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; } 
        public DateTime Date { get; set; }
    }
}
