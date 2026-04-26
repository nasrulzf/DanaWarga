using DanaWarga.Domain.Common;
using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Entities;

public sealed class IplPaymentAllocation : Entity
{
    public Guid PaymentId { get; private set; }
    public IplPayment? Payment { get; private set; }

    public int Year { get; private set; }
    public int Month { get; private set; }
    public Money AllocatedAmount { get; private set; } = Money.Zero();

    private IplPaymentAllocation()
    {
    }

    public IplPaymentAllocation(Guid paymentId, int year, int month, Money allocatedAmount)
    {
        PaymentId = paymentId;
        Year = year;
        Month = month;
        AllocatedAmount = allocatedAmount;
    }
}