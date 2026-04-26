using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Payments;
using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Queries.ListPayments;

public sealed class ListPaymentsQueryHandler(IIplPaymentRepository paymentRepository) : IRequestHandler<ListPaymentsQuery, IReadOnlyCollection<IplPaymentResult>>
{
    public async Task<IReadOnlyCollection<IplPaymentResult>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await paymentRepository.ListAsync(cancellationToken);
        return payments.Select(payment => new IplPaymentResult(
            payment.Id,
            payment.ResidentId,
            payment.HouseId,
            payment.TotalAmount.Amount,
            payment.PaymentDate,
            payment.Status.ToString(),
            payment.Allocations.Select(x => new PaymentAllocationResult(x.Year, x.Month, x.AllocatedAmount.Amount)).ToArray())).ToArray();
    }
}