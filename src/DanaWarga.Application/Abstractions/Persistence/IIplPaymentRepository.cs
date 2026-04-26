using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IIplPaymentRepository
{
    Task AddAsync(IplPayment payment, CancellationToken cancellationToken);
    Task<IplPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<IplPayment>> ListByHouseAsync(Guid houseId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<IplPayment>> ListAsync(CancellationToken cancellationToken);
}