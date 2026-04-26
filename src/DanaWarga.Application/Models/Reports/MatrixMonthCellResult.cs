namespace DanaWarga.Application.Models.Reports;

public sealed record MatrixMonthCellResult(int Year, int Month, MatrixStatusResult Status, decimal RequiredAmount, decimal AllocatedAmount);