using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Commands.CreateExpense;

public sealed class CreateExpenseCommandHandler(
    IExpenseRepository expenseRepository,
    IFinancialPeriodRepository financialPeriodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateExpenseCommand, Guid>
{
    public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        if (await financialPeriodRepository.IsClosedAsync(request.Date.Year, request.Date.Month, cancellationToken))
        {
            throw new InvalidOperationException("Cannot add expense to a closed financial period.");
        }

        var entity = new Expense(request.Date, request.Category, request.Description, new Money(request.Amount));
        await expenseRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}