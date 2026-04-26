namespace DanaWarga.Application.Models;

public readonly record struct PeriodKey(int Year, int Month) : IComparable<PeriodKey>
{
    public int CompareTo(PeriodKey other)
    {
        var yearCmp = Year.CompareTo(other.Year);
        return yearCmp != 0 ? yearCmp : Month.CompareTo(other.Month);
    }

    public DateTime ToDate() => new(Year, Month, 1);
}