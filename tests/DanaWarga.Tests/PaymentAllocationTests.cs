using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models;
using DanaWarga.Application.Features.IplPayments.Commands.CreatePayment;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using BillingPeriodEntity = DanaWarga.Domain.Entities.Period;

namespace DanaWarga.Tests;

public sealed class PaymentAllocationTests
{
    [Fact]
    public async Task Handle_ShouldAllocateFullPaymentAcrossPeriods()
    {
        var setup = BuildBaseSetup(180000m);
        await setup.Handler.Handle(new CreatePaymentCommand(setup.ResidentId, setup.HouseId, 360000m, DateTime.UtcNow, null), CancellationToken.None);

        var payment = setup.GetSavedPayment()!;

        payment.Allocations.Should().HaveCount(2);
        payment.Allocations.Should().Contain(x => x.Year == 2026 && x.Month == 1 && x.AllocatedAmount.Amount == 180000m);
        payment.Allocations.Should().Contain(x => x.Year == 2026 && x.Month == 2 && x.AllocatedAmount.Amount == 180000m);
    }

    [Fact]
    public async Task Handle_ShouldAllocatePartialPayment()
    {
        var setup = BuildBaseSetup(180000m);
        await setup.Handler.Handle(new CreatePaymentCommand(setup.ResidentId, setup.HouseId, 120000m, DateTime.UtcNow, null), CancellationToken.None);

        var payment = setup.GetSavedPayment()!;

        payment.Allocations.Should().ContainSingle();
        payment.Allocations.Single().AllocatedAmount.Amount.Should().Be(120000m);
    }

    [Fact]
    public async Task Handle_ShouldAdvanceOverpaymentToFuturePeriod()
    {
        var setup = BuildBaseSetup(180000m);
        setup.PaymentRepository
            .Setup(x => x.ListByHouseAsync(setup.HouseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                BuildValidatedPayment(setup.ResidentId, setup.HouseId, (2026, 1, 180000m), (2026, 2, 180000m))
            ]);

        await setup.Handler.Handle(new CreatePaymentCommand(setup.ResidentId, setup.HouseId, 400000m, DateTime.UtcNow, null), CancellationToken.None);

        var payment = setup.GetSavedPayment()!;

        payment.Allocations.Should().Contain(x => x.Month == 3 && x.AllocatedAmount.Amount == 180000m);
        payment.Allocations.Should().Contain(x => x.Month == 4 && x.AllocatedAmount.Amount == 180000m);
        payment.Allocations.Should().Contain(x => x.Month == 5 && x.AllocatedAmount.Amount == 40000m);
    }

    [Fact]
    public async Task Handle_ShouldHandleMultiPeriodAllocationFromOldest()
    {
        var setup = BuildBaseSetup(100000m);
        await setup.Handler.Handle(new CreatePaymentCommand(setup.ResidentId, setup.HouseId, 250000m, DateTime.UtcNow, null), CancellationToken.None);

        var payment = setup.GetSavedPayment()!;

        payment.Allocations.Select(x => new PeriodKey(x.Year, x.Month)).Should().ContainInOrder(
            new PeriodKey(2026, 1),
            new PeriodKey(2026, 2),
            new PeriodKey(2026, 3));
    }

    private static (CreatePaymentCommandHandler Handler, Guid HouseId, Guid ResidentId, Mock<IIplPaymentRepository> PaymentRepository, Func<IplPayment?> GetSavedPayment) BuildBaseSetup(decimal price)
    {
        var residentId = Guid.NewGuid();
        var houseId = Guid.NewGuid();
        IplPayment? savedPayment = null;

        var paymentRepository = new Mock<IIplPaymentRepository>();
        paymentRepository.Setup(x => x.ListByHouseAsync(houseId, It.IsAny<CancellationToken>())).ReturnsAsync([]);
        paymentRepository
            .Setup(x => x.AddAsync(It.IsAny<IplPayment>(), It.IsAny<CancellationToken>()))
            .Callback<IplPayment, CancellationToken>((payment, _) => savedPayment = payment)
            .Returns(Task.CompletedTask);

        var priceRepository = MockPriceConfigurationRepository(price, houseId);
        var financialPeriodRepository = new Mock<IFinancialPeriodRepository>();
        financialPeriodRepository.Setup(x => x.IsClosedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var handler = new CreatePaymentCommandHandler(
            paymentRepository.Object,
            MockHouseRepository(houseId).Object,
            MockResidentRepository(residentId).Object,
            MockPeriodRepository().Object,
            financialPeriodRepository.Object,
            priceRepository.Object,
            MockUnitOfWork().Object);

        return (handler, houseId, residentId, paymentRepository, () => savedPayment);
    }

    private static Mock<IHouseRepository> MockHouseRepository(Guid houseId)
    {
        var mock = new Mock<IHouseRepository>();
        mock.Setup(x => x.GetByIdAsync(houseId, It.IsAny<CancellationToken>())).ReturnsAsync(new House("A-01", "Address", Guid.NewGuid()));
        return mock;
    }

    private static Mock<IResidentRepository> MockResidentRepository(Guid residentId)
    {
        var mock = new Mock<IResidentRepository>();
        mock.Setup(x => x.GetByIdAsync(residentId, It.IsAny<CancellationToken>())).ReturnsAsync(new Resident("Resident", "resident@test.com", "0800"));
        return mock;
    }

    private static Mock<IPeriodRepository> MockPeriodRepository()
    {
        var mock = new Mock<IPeriodRepository>();
        mock.Setup(x => x.ListByHouseAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new BillingPeriodEntity(2026, 1),
                new BillingPeriodEntity(2026, 2),
                new BillingPeriodEntity(2026, 3),
                new BillingPeriodEntity(2026, 4),
                new BillingPeriodEntity(2026, 5),
                new BillingPeriodEntity(2026, 6)
            ]);
        return mock;
    }

    private static Mock<IIplPriceConfigurationRepository> MockPriceConfigurationRepository(decimal price, Guid houseId)
    {
        var mock = new Mock<IIplPriceConfigurationRepository>();
        mock.Setup(x => x.ListByHouseAsync(houseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new IplPriceConfiguration(houseId, new Money(price), new DateTime(2026, 1, 1), null)
            ]);
        return mock;
    }

    private static Mock<IUnitOfWork> MockUnitOfWork()
    {
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mock;
    }

    private static IplPayment BuildValidatedPayment(Guid residentId, Guid houseId, params (int year, int month, decimal amount)[] allocations)
    {
        var payment = new IplPayment(residentId, houseId, new Money(allocations.Sum(x => x.amount)), DateTime.UtcNow.AddDays(-1), null);
        payment.SetStatus(PaymentStatus.Validated);
        foreach (var allocation in allocations)
        {
            payment.AddAllocation(new IplPaymentAllocation(payment.Id, allocation.year, allocation.month, new Money(allocation.amount)));
        }

        return payment;
    }
}