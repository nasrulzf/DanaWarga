using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Commands.ValidatePayment;

public sealed class ValidatePaymentCommandHandler(
    IIplPaymentRepository paymentRepository,
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
            await incomeRepository.AddAsync(
                new Income(payment.PaymentDate, "IPL", $"Validated payment {payment.Id}", payment.TotalAmount),
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}