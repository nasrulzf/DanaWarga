namespace DanaWarga.Contracts.Finance;

public sealed record ExpenseDto(Guid Id, DateTime Date, string Category, string Description, decimal Amount);