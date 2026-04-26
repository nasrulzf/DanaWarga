namespace DanaWarga.Contracts.Finance;

public sealed record CreateIncomeRequest(DateTime Date, string Source, string Description, decimal Amount);