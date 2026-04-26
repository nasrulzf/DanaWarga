namespace DanaWarga.Application.Models.Finance;

public sealed record ExpenseResult(Guid Id, DateTime Date, string Category, string Description, decimal Amount);