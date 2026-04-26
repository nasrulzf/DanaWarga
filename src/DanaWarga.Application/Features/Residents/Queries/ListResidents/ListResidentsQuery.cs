using DanaWarga.Application.Models.Residents;
using MediatR;

namespace DanaWarga.Application.Features.Residents.Queries.ListResidents;

public sealed record ListResidentsQuery : IRequest<IReadOnlyCollection<ResidentResult>>;