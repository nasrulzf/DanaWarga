namespace DanaWarga.Contracts.Finance;

public sealed record IncomeDto(Guid Id, DateTime Date, string Source, string Description, decimal Amount);