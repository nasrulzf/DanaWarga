using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Commands.ValidatePayment;

public sealed class ValidatePaymentCommandHandler(
    IIplPaymentRepository paymentRepository,
    IFinancialPeriodRepository financialPeriodRepository,
    IIncomeRepository incomeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ValidatePaymentCommand, bool>
{
    public async Task<bool> Handle(ValidatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken)
            ?? throw new InvalidOperationException("Payment not found.");

        payment.SetStatus(request.Approve ? PaymentStatus.Validated : PaymentStatus.Rejected);

        if (request.Approve)
        {
            foreach (var allocation in payment.Allocations)
            {
                if (await financialPeriodRepository.IsClosedAsync(allocation.Year, allocation.Month, cancellationToken))
                {
                    throw new InvalidOperationException("Cannot validate payment allocation for a closed financial period.");
                }
            }

            await incomeRepository.AddAsync(
                new Income(payment.PaymentDate, "IPL", $"Validated payment {payment.Id}", payment.TotalAmount),
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}