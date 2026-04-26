using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using MediatR;

namespace DanaWarga.Application.Features.Residents.Commands.CreateResident;

public sealed class CreateResidentCommandHandler(
    IResidentRepository residentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateResidentCommand, Guid>
{
    public async Task<Guid> Handle(CreateResidentCommand request, CancellationToken cancellationToken)
    {
        var resident = new Resident(request.FullName, request.Email, request.PhoneNumber);
        await residentRepository.AddAsync(resident, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return resident.Id;
    }
}