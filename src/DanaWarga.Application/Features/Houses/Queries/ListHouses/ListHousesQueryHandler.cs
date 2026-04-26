using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Houses;
using MediatR;

namespace DanaWarga.Application.Features.Houses.Queries.ListHouses;

public sealed class ListHousesQueryHandler(IHouseRepository houseRepository) : IRequestHandler<ListHousesQuery, IReadOnlyCollection<HouseResult>>
{
    public async Task<IReadOnlyCollection<HouseResult>> Handle(ListHousesQuery request, CancellationToken cancellationToken)
    {
        var houses = await houseRepository.ListAsync(cancellationToken);
        return houses.Select(x => new HouseResult(x.Id, x.HouseNumber, x.Address, x.ResidentId, x.Resident?.FullName ?? string.Empty)).ToArray();
    }
}