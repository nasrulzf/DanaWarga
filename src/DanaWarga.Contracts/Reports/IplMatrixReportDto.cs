namespace DanaWarga.Contracts.Reports;

public sealed record IplMatrixReportDto(
    int Year,
    IReadOnlyCollection<MatrixRowDto> Rows,
    int TotalPaidHouses,
    int TotalUnpaidHouses,
    decimal CollectionRate);