using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using MediatR;

namespace DanaWarga.Application.Features.Finance.Commands.CreateIncome;

public sealed class CreateIncomeCommandHandler(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateIncomeCommand, Guid>
{
    public async Task<Guid> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Income(request.Date, request.Source, request.Description, new Money(request.Amount));
        await incomeRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}