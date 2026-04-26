using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IHouseRepository
{
    Task<IReadOnlyCollection<House>> ListAsync(CancellationToken cancellationToken);
    Task<House?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(House house, CancellationToken cancellationToken);
}