using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Commands.ValidatePayment;

public sealed record ValidatePaymentCommand(Guid PaymentId, bool Approve) : IRequest<bool>;