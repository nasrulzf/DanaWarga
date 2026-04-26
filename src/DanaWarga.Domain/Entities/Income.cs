using DanaWarga.Domain.Common;
using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Entities;

public sealed class Income : Entity
{
    public DateTime Date { get; private set; }
    public string Source { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = Money.Zero();

    private Income()
    {
    }

    public Income(DateTime date, string source, string description, Money amount)
    {
        Date = date;
        Source = source;
        Description = description;
        Amount = amount;
    }
}