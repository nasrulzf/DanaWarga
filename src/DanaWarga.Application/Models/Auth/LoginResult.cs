namespace DanaWarga.Application.Models.Auth;

public sealed record LoginResult(string AccessToken, DateTime ExpiresAtUtc, IReadOnlyCollection<string> Roles);