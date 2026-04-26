using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class FinancialPeriodSnapshotRepository(DanaWargaDbContext dbContext) : IFinancialPeriodSnapshotRepository
{
    public Task<FinancialPeriodSnapshot?> GetByYearMonthAsync(int year, int month, CancellationToken cancellationToken)
        => dbContext.FinancialPeriodSnapshots.FirstOrDefaultAsync(x => x.Year == year && x.Month == month, cancellationToken);

    public Task AddAsync(FinancialPeriodSnapshot snapshot, CancellationToken cancellationToken)
        => dbContext.FinancialPeriodSnapshots.AddAsync(snapshot, cancellationToken).AsTask();
}
