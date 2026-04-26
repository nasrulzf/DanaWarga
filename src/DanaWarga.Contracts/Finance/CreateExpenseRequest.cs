namespace DanaWarga.Contracts.Finance;

public sealed record CreateExpenseRequest(DateTime Date, string Category, string Description, decimal Amount);