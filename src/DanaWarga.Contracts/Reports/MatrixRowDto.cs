namespace DanaWarga.Contracts.Reports;

public sealed record MatrixRowDto(Guid HouseId, string HouseNumber, string ResidentName, IReadOnlyCollection<MatrixMonthCellDto> Months);