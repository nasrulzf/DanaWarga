using DanaWarga.Contracts.Reports;

namespace DanaWarga.WebMvc.Models;

public sealed class ReportsPageViewModel
{
    public int Year { get; set; }
    public IplMatrixReportDto? Report { get; set; }
}