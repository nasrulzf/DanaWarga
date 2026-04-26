using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Features.Reports.Queries.GetFinancialReport;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace DanaWarga.Tests.Application;

public sealed class FinancialReportQueryTests
{
    [Fact]
    public async Task Handle_ShouldUseSnapshotForClosedPeriod()
    {
        var period = new FinancialPeriod(2026, 4);
        period.Close(Guid.NewGuid(), DateTime.UtcNow.AddDays(-1));

        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.GetByYearMonthAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(period);

        var snapshotRepository = new Mock<IFinancialPeriodSnapshotRepository>();
        snapshotRepository.Setup(x => x.GetByYearMonthAsync(2026, 4, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FinancialPeriodSnapshot(2026, 4, new Money(700000m), new Money(200000m), new Money(500000m), "{}"));

        var incomeRepository = new Mock<IIncomeRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();

        var handler = new GetFinancialReportQueryHandler(periodRepository.Object, snapshotRepository.Object, incomeRepository.Object, expenseRepository.Object);
        var result = await handler.Handle(new GetFinancialReportQuery(2026, 4), CancellationToken.None);

        result.Source.Should().Be("snapshot");
        result.EndingBalance.Should().Be(500000m);
    }

    [Fact]
    public async Task Handle_ShouldUseLiveCalculationForOpenPeriod()
    {
        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.GetByYearMonthAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync((FinancialPeriod?)null);

        var snapshotRepository = new Mock<IFinancialPeriodSnapshotRepository>();
        var incomeRepository = new Mock<IIncomeRepository>();
        incomeRepository.Setup(x => x.GetTotalByPeriodAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(900000m);

        var expenseRepository = new Mock<IExpenseRepository>();
        expenseRepository.Setup(x => x.GetTotalByPeriodAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(300000m);

        var handler = new GetFinancialReportQueryHandler(periodRepository.Object, snapshotRepository.Object, incomeRepository.Object, expenseRepository.Object);
        var result = await handler.Handle(new GetFinancialReportQuery(2026, 4), CancellationToken.None);

        result.Source.Should().Be("live");
        result.IsClosed.Should().BeFalse();
        result.EndingBalance.Should().Be(600000m);
    }
}
