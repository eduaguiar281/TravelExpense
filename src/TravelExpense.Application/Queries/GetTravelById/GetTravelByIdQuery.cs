using TravelExpense.Application.Dtos;
using TravelExpense.Core;

namespace TravelExpense.Application.Queries.GetTravelById
{
    public class GetTravelByIdQuery : Query<QueryResult<TravelDto>>
    {
        public long Id { get; set; }
    }
}
