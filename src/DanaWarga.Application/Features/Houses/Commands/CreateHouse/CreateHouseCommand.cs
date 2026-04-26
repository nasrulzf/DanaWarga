using MediatR;

namespace DanaWarga.Application.Features.Houses.Commands.CreateHouse;

public sealed record CreateHouseCommand(string HouseNumber, string Address, Guid ResidentId) : IRequest<Guid>;