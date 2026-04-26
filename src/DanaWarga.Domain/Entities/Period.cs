using DanaWarga.Domain.Common;

namespace DanaWarga.Domain.Entities;

public sealed class Period : Entity
{
    public int Year { get; private set; }
    public int Month { get; private set; }

    private Period()
    {
    }

    public Period(int year, int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        Year = year;
        Month = month;
    }
}