using CSharpFunctionalExtensions;

namespace TravelExpense.Core.ExtensionsMethods
{
    public static class CommonValidations
    {
        public static Result FailIfNullOrEmpty(this string value, in string message = "Value cannot be null or empty!")
        {
            return Result.FailureIf(string.IsNullOrEmpty(value), message);
        }

        public static Result FailIfLessThanOrEquals(this DateTime value, in DateTime other, in string message)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value <= other, message);
        }

        public static Result FailIfLessThanOrEquals(this int value, in int other, in string message)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value <= other, message);
        }

        public static Result FailIfLessThanOrEquals(this decimal value, in decimal other, in string message)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value <= other, message);
        }

        public static Result FailIfLessThan(this DateTime value, in DateTime other, in string message)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value < other, message);
        }

        public static Result FailIfLessThan(this int value, in int other, in string message)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value < other, message);
        }
        public static Result FailIfLessThan(this decimal value, in decimal other, in string message)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value < other, message);
        }

        public static Result FailIfEmpty<T>(this IEnumerable<T> value, in string messsage = "Collection cannot be empty!")
        {
            return Result.SuccessIf(value.Any(), messsage);
        }

        public static Result ShouldBe<T>(this T value, in T other, in string message) where T : struct
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.SuccessIf(value.Equals(other), message);
        }

        public static Result ShouldNotBe<T>(this T value, in T other, in string message) where T : struct
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            return Result.FailureIf(value.Equals(other), message);
        }

    }
}
