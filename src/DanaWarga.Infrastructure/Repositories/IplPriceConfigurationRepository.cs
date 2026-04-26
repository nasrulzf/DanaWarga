using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class IplPriceConfigurationRepository(DanaWargaDbContext dbContext) : IIplPriceConfigurationRepository
{
    public async Task<IReadOnlyCollection<IplPriceConfiguration>> ListByHouseAsync(Guid houseId, CancellationToken cancellationToken)
    {
        return await dbContext.IplPriceConfigurations
            .Where(x => x.HouseId == houseId)
            .OrderBy(x => x.EffectiveStartDate)
            .ToArrayAsync(cancellationToken);
    }
}