using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class UserRepository(DanaWargaDbContext dbContext) : IUserRepository
{
    public Task<AppUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        => dbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

    public Task AddAsync(AppUser user, CancellationToken cancellationToken)
        => dbContext.Users.AddAsync(user, cancellationToken).AsTask();
}