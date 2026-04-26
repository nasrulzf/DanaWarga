using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Models;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using BillingPeriod = DanaWarga.Domain.ValueObjects.Period;

namespace DanaWarga.Tests.Domain;

public sealed class IplPaymentAggregateTests
{
    [Fact]
    public void AllocatePayment_ShouldAllocateFifo_WithPartialAndAdvanceSupport()
    {
        var payment = new IplPayment(Guid.NewGuid(), Guid.NewGuid(), new Money(400000m), DateTime.UtcNow, null);

        payment.AllocatePayment(
        [
            new PeriodOutstanding(new BillingPeriod(2026, 2), new Money(180000m)),
            new PeriodOutstanding(new BillingPeriod(2026, 3), new Money(180000m)),
            new PeriodOutstanding(new BillingPeriod(2026, 4), new Money(180000m))
        ]);

        payment.Allocations.Should().HaveCount(3);
        payment.Allocations.Should().Contain(x => x.Year == 2026 && x.Month == 2 && x.AllocatedAmount.Amount == 180000m);
        payment.Allocations.Should().Contain(x => x.Year == 2026 && x.Month == 3 && x.AllocatedAmount.Amount == 180000m);
        payment.Allocations.Should().Contain(x => x.Year == 2026 && x.Month == 4 && x.AllocatedAmount.Amount == 40000m);
    }
}