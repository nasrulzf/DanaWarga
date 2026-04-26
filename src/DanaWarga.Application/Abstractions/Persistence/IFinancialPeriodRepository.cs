using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IFinancialPeriodRepository
{
    Task<FinancialPeriod?> GetByYearMonthAsync(int year, int month, CancellationToken cancellationToken);
    Task<bool> IsClosedAsync(int year, int month, CancellationToken cancellationToken);
    Task AddAsync(FinancialPeriod period, CancellationToken cancellationToken);
}
