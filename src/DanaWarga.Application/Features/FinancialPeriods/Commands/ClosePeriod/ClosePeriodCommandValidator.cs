using FluentValidation;

namespace DanaWarga.Application.Features.FinancialPeriods.Commands.ClosePeriod;

public sealed class ClosePeriodCommandValidator : AbstractValidator<ClosePeriodCommand>
{
    public ClosePeriodCommandValidator()
    {
        RuleFor(x => x.Year).InclusiveBetween(2000, 3000);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
        RuleFor(x => x.ClosedBy).NotEmpty();
    }
}
