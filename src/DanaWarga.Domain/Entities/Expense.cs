using DanaWarga.Domain.Common;
using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Entities;

public sealed class Expense : Entity
{
    public DateTime Date { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = Money.Zero();

    private Expense()
    {
    }

    public Expense(DateTime date, string category, string description, Money amount)
    {
        Date = date;
        Category = category;
        Description = description;
        Amount = amount;
    }
}