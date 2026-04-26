using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Residents;
using MediatR;

namespace DanaWarga.Application.Features.Residents.Queries.ListResidents;

public sealed class ListResidentsQueryHandler(IResidentRepository residentRepository) : IRequestHandler<ListResidentsQuery, IReadOnlyCollection<ResidentResult>>
{
    public async Task<IReadOnlyCollection<ResidentResult>> Handle(ListResidentsQuery request, CancellationToken cancellationToken)
    {
        var residents = await residentRepository.ListAsync(cancellationToken);
        return residents.Select(x => new ResidentResult(x.Id, x.FullName, x.Email, x.PhoneNumber)).ToArray();
    }
}