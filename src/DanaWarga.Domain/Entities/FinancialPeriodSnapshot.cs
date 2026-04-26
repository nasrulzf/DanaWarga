using DanaWarga.Domain.Common;
using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Entities;

public sealed class FinancialPeriodSnapshot : Entity
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public Money TotalIncome { get; private set; } = Money.Zero();
    public Money TotalExpense { get; private set; } = Money.Zero();
    public Money EndingBalance { get; private set; } = Money.Zero();
    public string? JsonSnapshot { get; private set; }

    private FinancialPeriodSnapshot()
    {
    }

    public FinancialPeriodSnapshot(int year, int month, Money totalIncome, Money totalExpense, Money endingBalance, string? jsonSnapshot)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        Year = year;
        Month = month;
        TotalIncome = totalIncome;
        TotalExpense = totalExpense;
        EndingBalance = endingBalance;
        JsonSnapshot = jsonSnapshot;
    }
}
