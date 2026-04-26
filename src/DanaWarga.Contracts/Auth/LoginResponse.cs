namespace DanaWarga.Contracts.Auth;

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAtUtc, IReadOnlyCollection<string> Roles);