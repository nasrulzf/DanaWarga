using DanaWarga.Domain.Entities;

namespace DanaWarga.Application.Abstractions.Persistence;

public interface IIplPriceConfigurationRepository
{
    Task<IReadOnlyCollection<IplPriceConfiguration>> ListByHouseAsync(Guid houseId, CancellationToken cancellationToken);
}