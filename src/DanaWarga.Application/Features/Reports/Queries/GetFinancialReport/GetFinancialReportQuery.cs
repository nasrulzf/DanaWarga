using DanaWarga.Application.Models.Reports;
using MediatR;

namespace DanaWarga.Application.Features.Reports.Queries.GetFinancialReport;

public sealed record GetFinancialReportQuery(int Year, int Month) : IRequest<FinancialReportResult>;
