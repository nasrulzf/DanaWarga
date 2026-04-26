using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Security;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) Generate(AppUser user, IReadOnlyCollection<string> roles);
}