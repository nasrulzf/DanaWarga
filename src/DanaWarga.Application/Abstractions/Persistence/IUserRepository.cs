using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<AppUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task AddAsync(AppUser user, CancellationToken cancellationToken);
}