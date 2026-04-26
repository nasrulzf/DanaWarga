namespace DanaWarga.Application.Models.Reports;

public sealed record MatrixRowResult(Guid HouseId, string HouseNumber, string ResidentName, IReadOnlyCollection<MatrixMonthCellResult> Months);