namespace DanaWarga.Application.Models.Reports;

public sealed record IplMatrixReportResult(
    int Year,
    IReadOnlyCollection<MatrixRowResult> Rows,
    int TotalPaidHouses,
    int TotalUnpaidHouses,
    decimal CollectionRate);