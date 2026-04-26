using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IFinancialPeriodSnapshotRepository
{
    Task<FinancialPeriodSnapshot?> GetByYearMonthAsync(int year, int month, CancellationToken cancellationToken);
    Task AddAsync(FinancialPeriodSnapshot snapshot, CancellationToken cancellationToken);
}
