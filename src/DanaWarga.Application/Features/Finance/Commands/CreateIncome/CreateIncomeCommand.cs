using MediatR;

namespace DanaWarga.Application.Features.Finance.Commands.CreateIncome;

public sealed record CreateIncomeCommand(DateTime Date, string Source, string Description, decimal Amount) : IRequest<Guid>;