using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Commands.CreatePayment;

public sealed record CreatePaymentCommand(Guid ResidentId, Guid HouseId, decimal Amount, DateTime PaymentDate, string? ProofFilePath) : IRequest<Guid>;