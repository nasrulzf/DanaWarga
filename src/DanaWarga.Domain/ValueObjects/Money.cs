namespace DanaWarga.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "IDR")
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Money amount cannot be negative.");
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
        Currency = string.IsNullOrWhiteSpace(currency) ? "IDR" : currency.ToUpperInvariant();
    }

    public static Money Zero(string currency = "IDR") => new(0m, currency);
}