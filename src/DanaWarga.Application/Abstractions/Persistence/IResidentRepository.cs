using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IResidentRepository
{
    Task<IReadOnlyCollection<Resident>> ListAsync(CancellationToken cancellationToken);
    Task<Resident?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Resident resident, CancellationToken cancellationToken);
}