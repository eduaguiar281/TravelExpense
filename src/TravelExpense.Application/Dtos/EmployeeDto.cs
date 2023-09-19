using TravelExpense.Domain.ValueObjects;

namespace TravelExpense.Application.Dtos
{
    public record EmployeeDto(string Registration, string Name)
    {
        public static implicit operator EmployeeDto(Employee employee) => new(employee.Registration, employee.Name);
    }
}
