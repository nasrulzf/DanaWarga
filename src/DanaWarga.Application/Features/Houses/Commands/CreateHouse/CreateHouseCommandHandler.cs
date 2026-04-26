using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using MediatR;

namespace DanaWarga.Application.Features.Houses.Commands.CreateHouse;

public sealed class CreateHouseCommandHandler(
    IHouseRepository houseRepository,
    IResidentRepository residentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateHouseCommand, Guid>
{
    public async Task<Guid> Handle(CreateHouseCommand request, CancellationToken cancellationToken)
    {
        _ = await residentRepository.GetByIdAsync(request.ResidentId, cancellationToken)
            ?? throw new InvalidOperationException("Resident not found.");

        var house = new House(request.HouseNumber, request.Address, request.ResidentId);
        await houseRepository.AddAsync(house, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return house.Id;
    }
}