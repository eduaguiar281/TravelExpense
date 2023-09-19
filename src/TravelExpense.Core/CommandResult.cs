namespace TravelExpense.Core
{
    public sealed class CommandResult<T>
    {
        public CommandResult(T? data, bool isSuccess, string message)
        {
            Data = data;
            IsSuccess = isSuccess;
            IsFailure = !isSuccess;
            Message = message;
        }

        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
        public string Message { get; set; } = string.Empty;

        public static CommandResult<T> Success(T data, string message = "")
        {
            return new CommandResult<T>(data, true, message);
        }
        public static CommandResult<T> Failure(string message)
        {
            return new CommandResult<T>(default, false, message);
        }
    }
}
