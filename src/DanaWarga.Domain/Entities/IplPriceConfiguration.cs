using DanaWarga.Domain.Common;
using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Entities;

public sealed class IplPriceConfiguration : Entity
{
    public Guid HouseId { get; private set; }
    public House? House { get; private set; }

    public Money Amount { get; private set; } = Money.Zero();
    public DateTime EffectiveStartDate { get; private set; }
    public DateTime? EffectiveEndDate { get; private set; }

    private IplPriceConfiguration()
    {
    }

    public IplPriceConfiguration(Guid houseId, Money amount, DateTime effectiveStartDate, DateTime? effectiveEndDate)
    {
        if (effectiveEndDate.HasValue && effectiveEndDate < effectiveStartDate)
        {
            throw new ArgumentException("End date must be greater than or equal to start date.");
        }

        HouseId = houseId;
        Amount = amount;
        EffectiveStartDate = effectiveStartDate.Date;
        EffectiveEndDate = effectiveEndDate?.Date;
    }
}