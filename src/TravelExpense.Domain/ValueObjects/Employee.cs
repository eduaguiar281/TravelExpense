using CSharpFunctionalExtensions;

namespace TravelExpense.Domain.ValueObjects
{
    public class Employee 
    {
        private Employee (string registration, string name)
        {
            Registration = registration;
            Name = name;
        }

        public string Registration { get; private set; }
        public string Name { get; private set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Employee other)
                return false;

            if (other.Name == this.Name && other.Registration == this.Registration)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Registration, Name);
        }

        public static bool operator ==(Employee? left, Employee? right)
        {
            return EqualityComparer<Employee>.Default.Equals(left, right);
        }

        public static bool operator !=(Employee? left, Employee? right)
        {
            return !(left == right);
        }

        public static Result<Employee> Create(string registration, string name)
        {
            var result = Result.Combine(
                Result.FailureIf(string.IsNullOrEmpty(registration), "Registration must not be null or empty!"),
                Result.FailureIf(string.IsNullOrEmpty(name), "Name must not be null or empty!"));

            if (result.IsFailure)
                return result.ConvertFailure<Employee>();

            return Result.Success(new Employee(registration, name));
        }

    }
}
