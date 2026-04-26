using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Features.Finance.Commands.CreateIncome;
using DanaWarga.Application.Features.IplPayments.Commands.ValidatePayment;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace DanaWarga.Tests.Application;

public sealed class ImmutabilityTests
{
    [Fact]
    public async Task CreateIncome_ShouldFailWhenPeriodIsClosed()
    {
        var incomeRepository = new Mock<IIncomeRepository>();
        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.IsClosedAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new CreateIncomeCommandHandler(incomeRepository.Object, periodRepository.Object, unitOfWork.Object);

        var act = async () => await handler.Handle(new CreateIncomeCommand(new DateTime(2026, 4, 10), "IPL", "desc", 1000m), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*closed financial period*");
    }

    [Fact]
    public async Task ValidatePayment_ShouldFailWhenAnyAllocationPeriodIsClosed()
    {
        var payment = new IplPayment(Guid.NewGuid(), Guid.NewGuid(), new Money(200000m), DateTime.UtcNow, null);
        payment.AddAllocation(new IplPaymentAllocation(payment.Id, 2026, 4, new Money(200000m)));

        var paymentRepository = new Mock<IIplPaymentRepository>();
        paymentRepository.Setup(x => x.GetByIdAsync(payment.Id, It.IsAny<CancellationToken>())).ReturnsAsync(payment);

        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.IsClosedAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var incomeRepository = new Mock<IIncomeRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new ValidatePaymentCommandHandler(paymentRepository.Object, periodRepository.Object, incomeRepository.Object, unitOfWork.Object);

        var act = async () => await handler.Handle(new ValidatePaymentCommand(payment.Id, true), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*closed financial period*");
    }
}
