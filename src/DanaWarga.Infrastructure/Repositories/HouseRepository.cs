using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class HouseRepository(DanaWargaDbContext dbContext) : IHouseRepository
{
    public async Task<IReadOnlyCollection<House>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.Houses.Include(x => x.Resident).OrderBy(x => x.HouseNumber).ToArrayAsync(cancellationToken);

    public Task<House?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Houses.Include(x => x.Resident).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task AddAsync(House house, CancellationToken cancellationToken)
        => dbContext.Houses.AddAsync(house, cancellationToken).AsTask();
}