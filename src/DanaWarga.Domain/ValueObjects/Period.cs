namespace DanaWarga.Domain.ValueObjects;

public readonly record struct Period : IComparable<Period>
{
    public int Year { get; }
    public int Month { get; }

    public Period(int year, int month)
    {
        Year = year;
        Month = month;

        if (Month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(Month), "Month must be between 1 and 12.");
        }
    }

    public int CompareTo(Period other)
    {
        var yearCmp = Year.CompareTo(other.Year);
        return yearCmp != 0 ? yearCmp : Month.CompareTo(other.Month);
    }

    public DateTime ToDate() => new(Year, Month, 1);

    public Period NextMonth() => Month == 12 ? new Period(Year + 1, 1) : new Period(Year, Month + 1);
}