using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Commands.CreateTravel
{
    public class CreateTravelCommand: Command<CommandResult<TravelDto>>
    {
        public string Description { get; set; } = string.Empty;
        public string EmployeeRegistration { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime StartedIn { get; set; } 
        public DateTime EndedIn { get; set; }
    }
}
