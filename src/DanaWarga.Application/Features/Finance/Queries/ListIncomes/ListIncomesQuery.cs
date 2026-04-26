using DanaWarga.Application.Models.Finance;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Queries.ListIncomes;

public sealed record ListIncomesQuery : IRequest<IReadOnlyCollection<IncomeResult>>;