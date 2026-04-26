using DanaWarga.Domain.Common;
using DanaWarga.Domain.Enums;

namespace DanaWarga.Domain.Entities;

public sealed class FinancialPeriod : Entity
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public FinancialPeriodStatus Status { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public Guid? ClosedBy { get; private set; }

    private FinancialPeriod()
    {
    }

    public FinancialPeriod(int year, int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        Year = year;
        Month = month;
        Status = FinancialPeriodStatus.Open;
    }

    public void Close(Guid closedBy, DateTime closedAt)
    {
        if (Status == FinancialPeriodStatus.Closed)
        {
            throw new InvalidOperationException("Financial period is already closed.");
        }

        Status = FinancialPeriodStatus.Closed;
        ClosedBy = closedBy;
        ClosedAt = closedAt;
        Touch();
    }
}
