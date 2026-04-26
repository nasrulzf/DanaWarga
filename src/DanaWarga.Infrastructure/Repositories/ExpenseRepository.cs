using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class ExpenseRepository(DanaWargaDbContext dbContext) : IExpenseRepository
{
    public Task AddAsync(Expense expense, CancellationToken cancellationToken)
        => dbContext.Expenses.AddAsync(expense, cancellationToken).AsTask();

    public async Task<IReadOnlyCollection<Expense>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.Expenses.OrderByDescending(x => x.Date).ToArrayAsync(cancellationToken);

    public async Task<decimal> GetTotalByPeriodAsync(int year, int month, CancellationToken cancellationToken)
        => await dbContext.Expenses
            .Where(x => x.Date.Year == year && x.Date.Month == month)
            .SumAsync(x => x.Amount.Amount, cancellationToken);
}