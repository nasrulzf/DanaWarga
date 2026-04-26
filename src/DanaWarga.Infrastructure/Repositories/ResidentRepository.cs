using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class ResidentRepository(DanaWargaDbContext dbContext) : IResidentRepository
{
    public async Task<IReadOnlyCollection<Resident>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.Residents.OrderBy(x => x.FullName).ToArrayAsync(cancellationToken);

    public Task<Resident?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Residents.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task AddAsync(Resident resident, CancellationToken cancellationToken)
        => dbContext.Residents.AddAsync(resident, cancellationToken).AsTask();
}