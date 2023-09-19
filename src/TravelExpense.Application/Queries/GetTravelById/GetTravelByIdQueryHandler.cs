using CSharpFunctionalExtensions;
using MediatR;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Domain;
using TravelExpense.Infrastructure.Repositories;

namespace TravelExpense.Application.Queries.GetTravelById
{
    public class GetTravelByIdQueryHandler : IRequestHandler<GetTravelByIdQuery, QueryResult<TravelDto>>
    {

        private readonly ITravelRepository _travelRepository;

        public GetTravelByIdQueryHandler(ITravelRepository travelRepository)
        {
            _travelRepository = travelRepository;
        }

        public async Task<QueryResult<TravelDto>> Handle(GetTravelByIdQuery request, CancellationToken cancellationToken)
        {
            Maybe<Travel> maybeTravel = await _travelRepository.FindByIdAsync(request.Id, cancellationToken);

            if (!maybeTravel.HasValue)
                return QueryResult<TravelDto>.Failure("Travel not found!");

            return QueryResult<TravelDto>.Success(maybeTravel.Value);
        }
    }
}
