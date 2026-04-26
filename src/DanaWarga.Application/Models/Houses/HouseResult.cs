namespace DanaWarga.Application.Models.Houses;

public sealed record HouseResult(Guid Id, string HouseNumber, string Address, Guid ResidentId, string ResidentName);