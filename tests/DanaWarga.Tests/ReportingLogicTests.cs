using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Features.Reports.Queries.GetPaymentMatrix;
using DanaWarga.Application.Models.Reports;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace DanaWarga.Tests;

public sealed class ReportingLogicTests
{
    [Fact]
    public async Task Handle_ShouldMapPaidUnpaidAndEmpty()
    {
        var house = new House("A-01", "Address", Guid.NewGuid());
        var houseRepo = new Mock<IHouseRepository>();
        houseRepo.Setup(x => x.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync([house]);

        var payment = new IplPayment(house.ResidentId, house.Id, new Money(100000m), DateTime.UtcNow, null);
        payment.SetStatus(PaymentStatus.Validated);
        payment.AddAllocation(new IplPaymentAllocation(payment.Id, DateTime.UtcNow.Year, 1, new Money(100000m)));

        var paymentRepo = new Mock<IIplPaymentRepository>();
        paymentRepo.Setup(x => x.ListByHouseAsync(house.Id, It.IsAny<CancellationToken>())).ReturnsAsync([payment]);

        var pricingRepo = new Mock<IIplPriceConfigurationRepository>();
        pricingRepo.Setup(x => x.ListByHouseAsync(house.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new IplPriceConfiguration(house.Id, new Money(100000m), new DateTime(DateTime.UtcNow.Year, 1, 1), null)
            ]);

        var sut = new GetPaymentMatrixQueryHandler(houseRepo.Object, paymentRepo.Object, pricingRepo.Object);
        var result = await sut.Handle(new GetPaymentMatrixQuery(DateTime.UtcNow.Year), CancellationToken.None);

        var jan = result.Rows.Single().Months.Single(x => x.Month == 1);
        jan.Status.Should().Be(MatrixStatusResult.Paid);

        var current = result.Rows.Single().Months.Single(x => x.Month == DateTime.UtcNow.Month);
        if (DateTime.UtcNow.Month != 1)
        {
            current.Status.Should().Be(MatrixStatusResult.Unpaid);
        }

        var futureMonth = Math.Min(12, DateTime.UtcNow.Month + 1);
        if (futureMonth > DateTime.UtcNow.Month)
        {
            var future = result.Rows.Single().Months.Single(x => x.Month == futureMonth);
            future.Status.Should().Be(MatrixStatusResult.Empty);
        }
    }
}