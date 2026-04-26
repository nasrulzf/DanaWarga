using DanaWarga.Application.Features.Reports.Queries.GetPaymentMatrix;
using DanaWarga.Application.Features.Reports.Queries.GetFinancialReport;
using DanaWarga.Contracts.Finance;
using DanaWarga.Application.Models.Reports;
using DanaWarga.Contracts.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Treasurer,Committee")]
public sealed class ReportsController(ISender sender) : ControllerBase
{
    [HttpGet("ipl-matrix")]
    public async Task<IActionResult> IplMatrix([FromQuery] int year, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPaymentMatrixQuery(year), cancellationToken);
        return Ok(ToDto(result));
    }

    [HttpGet("financial")]
    public async Task<IActionResult> Financial([FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFinancialReportQuery(year, month), cancellationToken);
        return Ok(new FinancialReportDto(
            result.Year,
            result.Month,
            result.TotalIncome,
            result.TotalExpense,
            result.EndingBalance,
            result.IsClosed,
            result.ClosedAt,
            result.ClosedBy,
            result.Source));
    }

    private static IplMatrixReportDto ToDto(IplMatrixReportResult result)
    {
        return new IplMatrixReportDto(
            result.Year,
            result.Rows.Select(row => new MatrixRowDto(
                row.HouseId,
                row.HouseNumber,
                row.ResidentName,
                row.Months.Select(cell => new MatrixMonthCellDto(
                    cell.Year,
                    cell.Month,
                    cell.Status switch
                    {
                        MatrixStatusResult.Paid => MatrixStatus.Paid,
                        MatrixStatusResult.Unpaid => MatrixStatus.Unpaid,
                        _ => MatrixStatus.Empty
                    },
                    cell.RequiredAmount,
                    cell.AllocatedAmount)).ToArray())).ToArray(),
            result.TotalPaidHouses,
            result.TotalUnpaidHouses,
            result.CollectionRate);
    }
}