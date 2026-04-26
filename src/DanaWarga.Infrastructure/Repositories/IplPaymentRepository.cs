using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DanaWarga.Infrastructure.Repositories;

public sealed class IplPaymentRepository(DanaWargaDbContext dbContext) : IIplPaymentRepository
{
    public Task AddAsync(IplPayment payment, CancellationToken cancellationToken)
        => dbContext.IplPayments.AddAsync(payment, cancellationToken).AsTask();

    public Task<IplPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.IplPayments.Include(x => x.Allocations).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<IplPayment>> ListByHouseAsync(Guid houseId, CancellationToken cancellationToken)
        => await dbContext.IplPayments.Include(x => x.Allocations).Where(x => x.HouseId == houseId).ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<IplPayment>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.IplPayments.Include(x => x.Allocations).OrderByDescending(x => x.PaymentDate).ToArrayAsync(cancellationToken);
}