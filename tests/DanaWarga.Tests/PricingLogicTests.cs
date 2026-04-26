using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using BillingPeriod = DanaWarga.Domain.ValueObjects.Period;

namespace DanaWarga.Tests;

public sealed class PricingLogicTests
{
    [Fact]
    public void ResolvePrice_ShouldReturnCorrectPriceByPeriod()
    {
        var houseId = Guid.NewGuid();
        var configurations = new[]
        {
            new IplPriceConfiguration(houseId, new Money(150000m), new DateTime(2026, 1, 1), new DateTime(2026, 3, 31)),
            new IplPriceConfiguration(houseId, new Money(180000m), new DateTime(2026, 4, 1), null)
        };

        var sut = new DanaWarga.Domain.Services.IplPricingService();

        var jan = sut.ResolvePrice(configurations, new BillingPeriod(2026, 1), houseId);
        var apr = sut.ResolvePrice(configurations, new BillingPeriod(2026, 4), houseId);

        jan.Amount.Should().Be(150000m);
        apr.Amount.Should().Be(180000m);
    }

    [Fact]
    public void ResolvePrice_ShouldThrowWhenNoConfigFound()
    {
        var houseId = Guid.NewGuid();
        var sut = new DanaWarga.Domain.Services.IplPricingService();

        var act = () => sut.ResolvePrice([], new BillingPeriod(2026, 1), houseId);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ResolvePrice_ShouldUseNewestConfigWhenMultipleOverlap()
    {
        var houseId = Guid.NewGuid();
        var configurations = new[]
        {
            new IplPriceConfiguration(houseId, new Money(170000m), new DateTime(2026, 1, 1), null),
            new IplPriceConfiguration(houseId, new Money(190000m), new DateTime(2026, 5, 1), null)
        };

        var sut = new DanaWarga.Domain.Services.IplPricingService();
        var mayPrice = sut.ResolvePrice(configurations, new BillingPeriod(2026, 5), houseId);

        mayPrice.Amount.Should().Be(190000m);
    }
}