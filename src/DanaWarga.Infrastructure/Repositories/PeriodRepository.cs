using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class PeriodRepository(DanaWargaDbContext dbContext) : IPeriodRepository
{
    public async Task<IReadOnlyCollection<Period>> ListByHouseAsync(Guid houseId, CancellationToken cancellationToken)
    {
        var minDate = await dbContext.IplPriceConfigurations
            .Where(x => x.HouseId == houseId)
            .OrderBy(x => x.EffectiveStartDate)
            .Select(x => x.EffectiveStartDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (minDate == default)
        {
            return [];
        }

        var periods = await dbContext.Periods
            .Where(x => x.Year > minDate.Year || (x.Year == minDate.Year && x.Month >= minDate.Month))
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToArrayAsync(cancellationToken);

        return periods;
    }
}