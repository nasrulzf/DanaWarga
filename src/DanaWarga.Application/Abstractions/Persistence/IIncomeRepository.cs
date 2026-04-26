using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IIncomeRepository
{
    Task AddAsync(Income income, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Income>> ListAsync(CancellationToken cancellationToken);
    Task<decimal> GetTotalByPeriodAsync(int year, int month, CancellationToken cancellationToken);
}