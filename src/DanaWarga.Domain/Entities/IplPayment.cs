using DanaWarga.Domain.Common;
using DanaWarga.Domain.Enums;
using DanaWarga.Domain.Models;
using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Entities;

public sealed class IplPayment : Entity
{
    public Guid ResidentId { get; private set; }
    public Guid HouseId { get; private set; }
    public Money TotalAmount { get; private set; } = Money.Zero();
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? ProofFilePath { get; private set; }

    public ICollection<IplPaymentAllocation> Allocations { get; private set; } = new List<IplPaymentAllocation>();

    private IplPayment()
    {
    }

    public IplPayment(Guid residentId, Guid houseId, Money totalAmount, DateTime paymentDate, string? proofFilePath)
    {
        ResidentId = residentId;
        HouseId = houseId;
        TotalAmount = totalAmount;
        PaymentDate = paymentDate;
        ProofFilePath = proofFilePath;
        Status = PaymentStatus.Pending;
    }

    public void SetStatus(PaymentStatus status)
    {
        Status = status;
        Touch();
    }

    public void AddAllocation(IplPaymentAllocation allocation)
    {
        Allocations.Add(allocation);
    }

    public void AllocatePayment(IReadOnlyCollection<PeriodOutstanding> periods)
    {
        if (periods.Count == 0)
        {
            throw new InvalidOperationException("No period outstanding data available for allocation.");
        }

        if (Allocations.Count > 0)
        {
            throw new InvalidOperationException("Allocations already exist for this payment.");
        }

        var remaining = TotalAmount.Amount;
        foreach (var period in periods.OrderBy(x => x.Period))
        {
            if (remaining <= 0)
            {
                break;
            }

            if (period.OutstandingAmount.Amount <= 0)
            {
                continue;
            }

            var alloc = Math.Min(remaining, period.OutstandingAmount.Amount);
            AddAllocation(new IplPaymentAllocation(Id, period.Period.Year, period.Period.Month, new Money(alloc)));
            remaining -= alloc;
        }

        if (remaining > 0)
        {
            throw new InvalidOperationException("Allocation periods are insufficient to cover payment amount.");
        }
    }
}