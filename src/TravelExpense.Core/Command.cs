using MediatR;

namespace TravelExpense.Core
{
    public abstract class Command<T>: IRequest<T>
    {

    }
}
