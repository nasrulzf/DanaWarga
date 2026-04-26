using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Finance;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Queries.ListIncomes;

public sealed class ListIncomesQueryHandler(IIncomeRepository incomeRepository) : IRequestHandler<ListIncomesQuery, IReadOnlyCollection<IncomeResult>>
{
    public async Task<IReadOnlyCollection<IncomeResult>> Handle(ListIncomesQuery request, CancellationToken cancellationToken)
    {
        var data = await incomeRepository.ListAsync(cancellationToken);
        return data.Select(x => new IncomeResult(x.Id, x.Date, x.Source, x.Description, x.Amount.Amount)).ToArray();
    }
}