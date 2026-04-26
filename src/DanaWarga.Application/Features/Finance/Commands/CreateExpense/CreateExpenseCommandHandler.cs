using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Commands.CreateExpense;

public sealed class CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateExpenseCommand, Guid>
{
    public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Expense(request.Date, request.Category, request.Description, new Money(request.Amount));
        await expenseRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}