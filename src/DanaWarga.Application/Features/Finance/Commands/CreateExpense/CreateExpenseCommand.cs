using MediatR;

namespace DanaWarga.Application.Features.Finance.Commands.CreateExpense;

public sealed record CreateExpenseCommand(DateTime Date, string Category, string Description, decimal Amount) : IRequest<Guid>;