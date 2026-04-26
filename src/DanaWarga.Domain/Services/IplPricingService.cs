using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using BillingPeriod = DanaWarga.Domain.ValueObjects.Period;

namespace DanaWarga.Domain.Services;

public sealed class IplPricingService
{
    public Money ResolvePrice(IEnumerable<IplPriceConfiguration> configurations, BillingPeriod period, Guid houseId)
    {
        var date = period.ToDate();
        var config = configurations
            .Where(x => x.EffectiveStartDate <= date && (!x.EffectiveEndDate.HasValue || x.EffectiveEndDate >= date))
            .OrderByDescending(x => x.EffectiveStartDate)
            .FirstOrDefault();

        if (config is null)
        {
            throw new InvalidOperationException($"No IPL price configured for house {houseId} and period {period.Year}-{period.Month:00}.");
        }

        return config.Amount;
    }
}