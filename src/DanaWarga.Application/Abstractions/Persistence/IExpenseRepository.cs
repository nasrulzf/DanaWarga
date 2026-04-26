using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IExpenseRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Expense>> ListAsync(CancellationToken cancellationToken);
    Task<decimal> GetTotalByPeriodAsync(int year, int month, CancellationToken cancellationToken);
}