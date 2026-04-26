using DanaWarga.Application.Features.IplPayments.Commands.CreatePayment;
using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using BillingPeriodEntity = DanaWarga.Domain.Entities.Period;

namespace DanaWarga.Tests.Application;

public sealed class CreatePaymentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCreatedPaymentId()
    {
        var houseId = Guid.NewGuid();
        var residentId = Guid.NewGuid();
        IplPayment? saved = null;

        var paymentRepository = new Mock<IIplPaymentRepository>();
        paymentRepository.Setup(x => x.ListByHouseAsync(houseId, It.IsAny<CancellationToken>())).ReturnsAsync([]);
        paymentRepository
            .Setup(x => x.AddAsync(It.IsAny<IplPayment>(), It.IsAny<CancellationToken>()))
            .Callback<IplPayment, CancellationToken>((payment, _) => saved = payment)
            .Returns(Task.CompletedTask);

        var houseRepository = new Mock<IHouseRepository>();
        houseRepository.Setup(x => x.GetByIdAsync(houseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new House("A-01", "Addr", residentId));

        var residentRepository = new Mock<IResidentRepository>();
        residentRepository.Setup(x => x.GetByIdAsync(residentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Resident("Resident", "08123", "Street"));

        var periodRepository = new Mock<IPeriodRepository>();
        periodRepository.Setup(x => x.ListByHouseAsync(houseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new BillingPeriodEntity(2026, 1)]);

        var priceRepository = new Mock<IIplPriceConfigurationRepository>();
        priceRepository.Setup(x => x.ListByHouseAsync(houseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new IplPriceConfiguration(houseId, new Money(200000m), new DateTime(2026, 1, 1), null)]);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreatePaymentCommandHandler(
            paymentRepository.Object,
            houseRepository.Object,
            residentRepository.Object,
            periodRepository.Object,
            priceRepository.Object,
            unitOfWork.Object);

        var result = await handler.Handle(new CreatePaymentCommand(residentId, houseId, 200000m, DateTime.UtcNow, null), CancellationToken.None);

        result.Should().Be(saved!.Id);
    }
}