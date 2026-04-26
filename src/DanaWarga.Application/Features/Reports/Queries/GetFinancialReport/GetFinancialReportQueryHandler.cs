using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Reports;
using DanaWarga.Domain.Enums;
using MediatR;

namespace DanaWarga.Application.Features.Reports.Queries.GetFinancialReport;

public sealed class GetFinancialReportQueryHandler(
    IFinancialPeriodRepository financialPeriodRepository,
    IFinancialPeriodSnapshotRepository snapshotRepository,
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository) : IRequestHandler<GetFinancialReportQuery, FinancialReportResult>
{
    public async Task<FinancialReportResult> Handle(GetFinancialReportQuery request, CancellationToken cancellationToken)
    {
        var period = await financialPeriodRepository.GetByYearMonthAsync(request.Year, request.Month, cancellationToken);
        if (period is not null && period.Status == FinancialPeriodStatus.Closed)
        {
            var snapshot = await snapshotRepository.GetByYearMonthAsync(request.Year, request.Month, cancellationToken)
                ?? throw new InvalidOperationException("Closed period must have snapshot data.");

            return new FinancialReportResult(
                request.Year,
                request.Month,
                snapshot.TotalIncome.Amount,
                snapshot.TotalExpense.Amount,
                snapshot.EndingBalance.Amount,
                true,
                period.ClosedAt,
                period.ClosedBy,
                "snapshot");
        }

        var totalIncome = await incomeRepository.GetTotalByPeriodAsync(request.Year, request.Month, cancellationToken);
        var totalExpense = await expenseRepository.GetTotalByPeriodAsync(request.Year, request.Month, cancellationToken);

        return new FinancialReportResult(
            request.Year,
            request.Month,
            totalIncome,
            totalExpense,
            totalIncome - totalExpense,
            false,
            null,
            null,
            "live");
    }
}
