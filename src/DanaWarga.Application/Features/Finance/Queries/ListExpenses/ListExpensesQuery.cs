using DanaWarga.Application.Models.Finance;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Queries.ListExpenses;

public sealed record ListExpensesQuery : IRequest<IReadOnlyCollection<ExpenseResult>>;