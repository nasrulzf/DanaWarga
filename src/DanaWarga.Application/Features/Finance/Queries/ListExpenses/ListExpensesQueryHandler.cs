using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Finance;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Queries.ListExpenses;

public sealed class ListExpensesQueryHandler(IExpenseRepository expenseRepository) : IRequestHandler<ListExpensesQuery, IReadOnlyCollection<ExpenseResult>>
{
    public async Task<IReadOnlyCollection<ExpenseResult>> Handle(ListExpensesQuery request, CancellationToken cancellationToken)
    {
        var data = await expenseRepository.ListAsync(cancellationToken);
        return data.Select(x => new ExpenseResult(x.Id, x.Date, x.Category, x.Description, x.Amount.Amount)).ToArray();
    }
}