namespace DanaWarga.Application.Models.Finance;

public sealed record IncomeResult(Guid Id, DateTime Date, string Source, string Description, decimal Amount);