namespace DanaWarga.Contracts.Reports;

public sealed record MatrixMonthCellDto(int Year, int Month, MatrixStatus Status, decimal RequiredAmount, decimal AllocatedAmount);