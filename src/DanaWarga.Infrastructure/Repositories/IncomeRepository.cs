using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class IncomeRepository(DanaWargaDbContext dbContext) : IIncomeRepository
{
    public Task AddAsync(Income income, CancellationToken cancellationToken)
        => dbContext.Incomes.AddAsync(income, cancellationToken).AsTask();

    public async Task<IReadOnlyCollection<Income>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.Incomes.OrderByDescending(x => x.Date).ToArrayAsync(cancellationToken);
}