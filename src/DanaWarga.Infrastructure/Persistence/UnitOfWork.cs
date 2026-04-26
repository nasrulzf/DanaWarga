using DanaWarga.Application.Abstractions.Persistence;

namespace DanaWarga.Infrastructure.Persistence;

public sealed class UnitOfWork(DanaWargaDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}