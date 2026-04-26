using MediatR;

namespace DanaWarga.Application.Features.FinancialPeriods.Commands.ClosePeriod;

public sealed record ClosePeriodCommand(int Year, int Month, Guid ClosedBy) : IRequest<Guid>;
