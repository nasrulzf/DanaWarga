using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Features.FinancialPeriods.Commands.ClosePeriod;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DanaWarga.Tests.Application;

public sealed class PeriodClosingTests
{
    [Fact]
    public async Task Handle_ShouldCloseOpenPeriodAndCreateSnapshot()
    {
        var period = new FinancialPeriod(2026, 4);
        FinancialPeriodSnapshot? savedSnapshot = null;

        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.GetByYearMonthAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(period);

        var snapshotRepository = new Mock<IFinancialPeriodSnapshotRepository>();
        snapshotRepository.Setup(x => x.GetByYearMonthAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync((FinancialPeriodSnapshot?)null);
        snapshotRepository
            .Setup(x => x.AddAsync(It.IsAny<FinancialPeriodSnapshot>(), It.IsAny<CancellationToken>()))
            .Callback<FinancialPeriodSnapshot, CancellationToken>((snapshot, _) => savedSnapshot = snapshot)
            .Returns(Task.CompletedTask);

        var incomeRepository = new Mock<IIncomeRepository>();
        incomeRepository.Setup(x => x.GetTotalByPeriodAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(900000m);

        var expenseRepository = new Mock<IExpenseRepository>();
        expenseRepository.Setup(x => x.GetTotalByPeriodAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(300000m);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new ClosePeriodCommandHandler(
            periodRepository.Object,
            snapshotRepository.Object,
            incomeRepository.Object,
            expenseRepository.Object,
            unitOfWork.Object);

        var result = await handler.Handle(new ClosePeriodCommand(2026, 4, Guid.NewGuid()), CancellationToken.None);

        result.Should().Be(period.Id);
        period.Status.Should().Be(FinancialPeriodStatus.Closed);
        savedSnapshot.Should().NotBeNull();
        savedSnapshot!.EndingBalance.Amount.Should().Be(600000m);
    }

    [Fact]
    public async Task Handle_ShouldPreventDoubleClosing()
    {
        var period = new FinancialPeriod(2026, 4);
        period.Close(Guid.NewGuid(), DateTime.UtcNow);

        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.GetByYearMonthAsync(2026, 4, It.IsAny<CancellationToken>())).ReturnsAsync(period);

        var snapshotRepository = new Mock<IFinancialPeriodSnapshotRepository>();
        var incomeRepository = new Mock<IIncomeRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new ClosePeriodCommandHandler(
            periodRepository.Object,
            snapshotRepository.Object,
            incomeRepository.Object,
            expenseRepository.Object,
            unitOfWork.Object);

        var act = async () => await handler.Handle(new ClosePeriodCommand(2026, 4, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*already closed*");
    }

    [Fact]
    public async Task Handle_ShouldCreateOpenPeriodWhenMissingThenClose()
    {
        FinancialPeriod? addedPeriod = null;

        var periodRepository = new Mock<IFinancialPeriodRepository>();
        periodRepository.Setup(x => x.GetByYearMonthAsync(2026, 5, It.IsAny<CancellationToken>())).ReturnsAsync((FinancialPeriod?)null);
        periodRepository
            .Setup(x => x.AddAsync(It.IsAny<FinancialPeriod>(), It.IsAny<CancellationToken>()))
            .Callback<FinancialPeriod, CancellationToken>((period, _) => addedPeriod = period)
            .Returns(Task.CompletedTask);

        var snapshotRepository = new Mock<IFinancialPeriodSnapshotRepository>();
        snapshotRepository.Setup(x => x.GetByYearMonthAsync(2026, 5, It.IsAny<CancellationToken>())).ReturnsAsync((FinancialPeriodSnapshot?)null);
        snapshotRepository.Setup(x => x.AddAsync(It.IsAny<FinancialPeriodSnapshot>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var incomeRepository = new Mock<IIncomeRepository>();
        incomeRepository.Setup(x => x.GetTotalByPeriodAsync(2026, 5, It.IsAny<CancellationToken>())).ReturnsAsync(0m);

        var expenseRepository = new Mock<IExpenseRepository>();
        expenseRepository.Setup(x => x.GetTotalByPeriodAsync(2026, 5, It.IsAny<CancellationToken>())).ReturnsAsync(0m);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new ClosePeriodCommandHandler(
            periodRepository.Object,
            snapshotRepository.Object,
            incomeRepository.Object,
            expenseRepository.Object,
            unitOfWork.Object);

        await handler.Handle(new ClosePeriodCommand(2026, 5, Guid.NewGuid()), CancellationToken.None);

        addedPeriod.Should().NotBeNull();
        addedPeriod!.Status.Should().Be(FinancialPeriodStatus.Closed);
    }
}
