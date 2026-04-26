using DanaWarga.Application.Models.Houses;
using MediatR;

namespace DanaWarga.Application.Features.Houses.Queries.ListHouses;

public sealed record ListHousesQuery : IRequest<IReadOnlyCollection<HouseResult>>;