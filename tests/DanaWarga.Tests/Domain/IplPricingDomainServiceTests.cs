using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Services;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using BillingPeriod = DanaWarga.Domain.ValueObjects.Period;

namespace DanaWarga.Tests.Domain;

public sealed class IplPricingDomainServiceTests
{
    [Fact]
    public void ResolvePrice_ShouldReturnCorrectHistoricalPrice()
    {
        var houseId = Guid.NewGuid();
        var sut = new IplPricingService();
        var configs = new[]
        {
            new IplPriceConfiguration(houseId, new Money(150000m), new DateTime(2026, 1, 1), new DateTime(2026, 3, 31)),
            new IplPriceConfiguration(houseId, new Money(180000m), new DateTime(2026, 4, 1), null)
        };

        var march = sut.ResolvePrice(configs, new BillingPeriod(2026, 3), houseId);
        var april = sut.ResolvePrice(configs, new BillingPeriod(2026, 4), houseId);

        march.Amount.Should().Be(150000m);
        april.Amount.Should().Be(180000m);
    }
}