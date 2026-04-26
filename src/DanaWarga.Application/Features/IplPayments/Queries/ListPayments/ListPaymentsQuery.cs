using DanaWarga.Application.Models.Payments;
using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Queries.ListPayments;

public sealed record ListPaymentsQuery : IRequest<IReadOnlyCollection<IplPaymentResult>>;