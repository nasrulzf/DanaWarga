using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class FinancialPeriodRepository(DanaWargaDbContext dbContext) : IFinancialPeriodRepository
{
    public Task<FinancialPeriod?> GetByYearMonthAsync(int year, int month, CancellationToken cancellationToken)
        => dbContext.FinancialPeriods.FirstOrDefaultAsync(x => x.Year == year && x.Month == month, cancellationToken);

    public Task<bool> IsClosedAsync(int year, int month, CancellationToken cancellationToken)
        => dbContext.FinancialPeriods.AnyAsync(
            x => x.Year == year && x.Month == month && x.Status == FinancialPeriodStatus.Closed,
            cancellationToken);

    public Task AddAsync(FinancialPeriod period, CancellationToken cancellationToken)
        => dbContext.FinancialPeriods.AddAsync(period, cancellationToken).AsTask();
}
