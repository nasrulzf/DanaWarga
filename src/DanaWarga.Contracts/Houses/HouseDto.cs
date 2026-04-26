namespace DanaWarga.Contracts.Houses;

public sealed record HouseDto(Guid Id, string HouseNumber, string Address, Guid ResidentId, string ResidentName);