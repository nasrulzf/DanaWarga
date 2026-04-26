using MediatR;

namespace DanaWarga.Application.Features.Residents.Commands.CreateResident;

public sealed record CreateResidentCommand(string FullName, string Email, string PhoneNumber) : IRequest<Guid>;