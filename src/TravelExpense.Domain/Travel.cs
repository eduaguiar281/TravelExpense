using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using TravelExpense.Core;
using TravelExpense.Domain.Enums;
using TravelExpense.Domain.Events;
using TravelExpense.Domain.ValueObjects;

namespace TravelExpense.Domain
{
    public class Travel: DomainEntity, IAggregateRoot
    {
        private Travel()
            : base()
        {

        }

        private Travel(string description, Employee employee, DateTime startedIn, DateTime endedIn, TravelCreatedEvent createdEvent)
        {
            Description = description;
            Employee = employee;
            StartedIn = startedIn;
            EndedIn = endedIn;
            Status = TravelStatus.Open;
            TotalExpenses = 0;
            _events.Add(createdEvent!);
        }

        private readonly List<DomainEvent> _events = new ();

        public string? Description { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime StartedIn { get; private set; }
        public DateTime EndedIn { get; private set;}
        public TravelStatus Status { get ; private set; }
        public decimal TotalExpenses { get; private set; }

        private readonly List<Expense> _expenses = new();
        public IEnumerable<Expense> Expenses 
        { 
            get => _expenses
                .OrderBy(e => e.Date)
                .ToList(); 
        }

        [JsonIgnore]
        public IReadOnlyCollection<DomainEvent> Events { get => _events.AsReadOnly(); }

        public Result RegisterExpense(string relatedTo, string description, decimal value, DateTime date)
        {
            if (Status == TravelStatus.Closed)
                return Result.Failure("Travel has been closed, you don't add expense!");

            Result<Expense> result = Expense.Create(relatedTo, description, value, date);
            if (result.IsFailure)
                return result.ConvertFailure();

            Expense expense = result.Value;
            _expenses.Add(expense);
            TotalExpenses += expense.Value;

            _events.Add(new ExpenseRegisteredEvent 
            { 
                RelatedTo = relatedTo,
                Description = description,
                Value = value,
                Date = date,
            });
            return result;
        }

        public Result RemoveExpense(long expenseId)
        {
            var result = ExpenseExists(expenseId);
            if (result.IsFailure)
                return result;

            Expense expense = _expenses.FirstOrDefault(f => f.Id == expenseId)!;
            TotalExpenses -= expense.Value;
            _expenses.Remove(expense);
            _events.Add(new ExpenseRemovedEvent
            {
                ExpenseId = expenseId
            });
            return Result.Success();
        }

        public Result Aprove(long expenseId, string voucherId, string comment)
        {
            var result = ExpenseExists(expenseId);
            if (result.IsFailure)
                return result;

            Expense expense = _expenses.Find(f => f.Id == expenseId)!;
            result = expense.Aprove(voucherId, comment);
            if (result.IsSuccess)
                _events.Add(new ExpenseAprovedEvent
                {
                    ExpenseId = expenseId,
                    VoucherId = voucherId,
                    Comment = comment
                });

            return result;
        }

        public Result Reject(long expenseId, string comment)
        {
            var result = ExpenseExists(expenseId);
            if (result.IsFailure)
                return result;

            Expense expense = _expenses.Find(f => f.Id == expenseId)!;
            result = expense.Reject(comment);
            if (result.IsSuccess)
                _events.Add(new ExpenseRejectedEvent
                {
                    ExpenseId = expenseId,
                    Comment = comment
                });
            return result;
        }

        private Result ExpenseExists(long expenseId)
        {
            return Result.FailureIf(!_expenses.Any(e => e.Id == expenseId), "Expense not found!");
        }

        public static Result<Travel> Create(string description, Employee employee, DateTime startedIn, DateTime endedIn)
        {
            var result = Result.Combine(
                Result.FailureIf(string.IsNullOrEmpty(description), "Description must not be null or empty!"),
                Result.FailureIf(employee is null, "Employee must not be null"),
                Result.FailureIf(endedIn < startedIn, "EndedIn must be greater than or equal to StartedIn"));

            if (result.IsFailure)
                return result.ConvertFailure<Travel>();
            
            var createdEvent = new TravelCreatedEvent
            {
                Description = description,
                Employee = employee!,
                StartedIn = startedIn,
                EndedIn = endedIn
            };
            return Result.Success(new Travel(description, employee!, startedIn, endedIn, createdEvent));
        }
    }
}
