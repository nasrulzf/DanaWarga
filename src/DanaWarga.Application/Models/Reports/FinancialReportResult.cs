namespace DanaWarga.Application.Models.Reports;

public sealed record FinancialReportResult(
    int Year,
    int Month,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal EndingBalance,
    bool IsClosed,
    DateTime? ClosedAt,
    Guid? ClosedBy,
    string Source);
