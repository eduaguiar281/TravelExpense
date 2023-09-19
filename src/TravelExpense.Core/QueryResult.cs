using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpense.Core
{
    public sealed class QueryResult<T>
    {
        public QueryResult(T? data, bool isSuccess, string message)
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

        public static QueryResult<T> Success(T data, string message = "")
        {
            return new QueryResult<T>(data, true, message);
        }
        public static QueryResult<T> Failure(string message)
        {
            return new QueryResult<T>(default, false, message);
        }

    }
}
