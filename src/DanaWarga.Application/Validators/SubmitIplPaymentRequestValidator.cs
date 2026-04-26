using DanaWarga.Application.Features.IplPayments.Commands.CreatePayment;
using FluentValidation;

namespace DanaWarga.Application.Validators;

public sealed class SubmitIplPaymentRequestValidator : AbstractValidator<CreatePaymentCommand>
{
    public SubmitIplPaymentRequestValidator()
    {
        RuleFor(x => x.ResidentId).NotEmpty();
        RuleFor(x => x.HouseId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.PaymentDate).NotEmpty();
    }
}