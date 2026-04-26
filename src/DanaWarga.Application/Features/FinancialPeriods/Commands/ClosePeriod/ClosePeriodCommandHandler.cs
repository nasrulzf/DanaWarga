using System.Text.Json;
using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using DanaWarga.Domain.ValueObjects;
using MediatR;

namespace DanaWarga.Application.Features.FinancialPeriods.Commands.ClosePeriod;

public sealed class ClosePeriodCommandHandler(
    IFinancialPeriodRepository financialPeriodRepository,
    IFinancialPeriodSnapshotRepository snapshotRepository,
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ClosePeriodCommand, Guid>
{
    public async Task<Guid> Handle(ClosePeriodCommand request, CancellationToken cancellationToken)
    {
        var period = await financialPeriodRepository.GetByYearMonthAsync(request.Year, request.Month, cancellationToken);
        if (period is not null && period.Status == FinancialPeriodStatus.Closed)
        {
            throw new InvalidOperationException("Financial period is already closed.");
        }

        var existingSnapshot = await snapshotRepository.GetByYearMonthAsync(request.Year, request.Month, cancellationToken);
        if (existingSnapshot is not null)
        {
            throw new InvalidOperationException("Financial snapshot for this period already exists.");
        }

        var totalIncome = await incomeRepository.GetTotalByPeriodAsync(request.Year, request.Month, cancellationToken);
        var totalExpense = await expenseRepository.GetTotalByPeriodAsync(request.Year, request.Month, cancellationToken);
        var endingBalance = totalIncome - totalExpense;

        var payload = JsonSerializer.Serialize(new
        {
            request.Year,
            request.Month,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            EndingBalance = endingBalance,
            GeneratedAtUtc = DateTime.UtcNow
        });

        var snapshot = new FinancialPeriodSnapshot(
            request.Year,
            request.Month,
            new Money(totalIncome),
            new Money(totalExpense),
            new Money(endingBalance),
            payload);

        await snapshotRepository.AddAsync(snapshot, cancellationToken);

        if (period is null)
        {
            period = new FinancialPeriod(request.Year, request.Month);
            await financialPeriodRepository.AddAsync(period, cancellationToken);
        }

        period.Close(request.ClosedBy, DateTime.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return period.Id;
    }
}
