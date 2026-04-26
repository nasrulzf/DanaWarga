namespace DanaWarga.Contracts.Houses;

public sealed record CreateHouseRequest(string HouseNumber, string Address, Guid ResidentId);