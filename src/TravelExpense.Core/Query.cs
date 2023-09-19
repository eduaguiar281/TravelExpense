using MediatR;

namespace TravelExpense.Core
{
    public abstract class Query<T> : IRequest<T>
    {
    }
}
