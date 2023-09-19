using CSharpFunctionalExtensions;
using TravelExpense.Core;
using TravelExpense.Domain.Enums;

namespace TravelExpense.Domain
{
    public class Expense : DomainEntity
    {
        private Expense(string relatedTo, string description, decimal value, DateTime date) 
        {
            RelatedTo = relatedTo;
            Description = description;
            Value = value;
            Date = date;
            Status = ExpenseStatus.Registered;
        }

        public string RelatedTo { get; private set; }
        public string Description { get; private set; }
        public string? VoucherId { get; private set; }
        public decimal Value { get; private set; }
        public DateTime Date { get; private set; }
        public ExpenseStatus Status { get; private set; }
        public string? Comments { get; private set; }


        public Result Aprove(string voucherId, string comment)
        {
            var result = Result.FailureIf(string.IsNullOrEmpty(voucherId), "VoucherId cannot be null or empty");
            if (result.IsFailure)
                return result;
            
            VoucherId= voucherId;
            Comments += comment + "\r\n";
            Status = ExpenseStatus.Aproved;
            return result;
        }


        public Result Reject(string comment)
        {
            var result = Result.FailureIf(string.IsNullOrEmpty(comment), "Comment cannot be null or empty");
            if (result.IsFailure)
                return result;

            Comments += comment + "\r\n";
            Status = ExpenseStatus.Rejected;
            return Result.Success();
        }

        public static Result<Expense> Create(string relatedTo, string description, decimal value, DateTime date)
        {
            var result = Result.Combine(
                Result.FailureIf(string.IsNullOrEmpty(relatedTo), "RelatedTo cannot be null or empty"),
                Result.FailureIf(string.IsNullOrEmpty(description), "Description cannot be null or empty"),
                Result.FailureIf(value <= 0, "Value cannot be less than or equal to zero"));

            if (result.IsFailure)
                return result.ConvertFailure<Expense>();

            return Result.Success(new Expense(relatedTo, description, value, date));
        }
    }
}
