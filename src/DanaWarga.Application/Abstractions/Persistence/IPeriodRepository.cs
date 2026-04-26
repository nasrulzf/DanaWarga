using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IPeriodRepository
{
    Task<IReadOnlyCollection<Period>> ListByHouseAsync(Guid houseId, CancellationToken cancellationToken);
}