namespace DanaWarga.Contracts.Residents;

public sealed record ResidentDto(Guid Id, string FullName, string Email, string PhoneNumber);