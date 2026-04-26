using FluentValidation;

namespace DanaWarga.Application.Features.Reports.Queries.GetFinancialReport;

public sealed class GetFinancialReportQueryValidator : AbstractValidator<GetFinancialReportQuery>
{
    public GetFinancialReportQueryValidator()
    {
        RuleFor(x => x.Year).InclusiveBetween(2000, 3000);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
    }
}
