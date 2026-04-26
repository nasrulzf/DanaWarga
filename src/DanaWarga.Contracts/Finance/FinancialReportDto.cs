namespace DanaWarga.Contracts.Finance;

public sealed record FinancialReportDto(
    int Year,
    int Month,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal EndingBalance,
    bool IsClosed,
    DateTime? ClosedAt,
    Guid? ClosedBy,
    string Source);
