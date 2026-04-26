namespace DanaWarga.Contracts.Residents;

public sealed record CreateResidentRequest(string FullName, string Email, string PhoneNumber);