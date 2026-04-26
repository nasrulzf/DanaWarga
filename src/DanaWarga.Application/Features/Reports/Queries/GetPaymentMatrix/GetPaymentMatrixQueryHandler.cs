using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Models.Reports;
using DanaWarga.Domain.Enums;
using DanaWarga.Domain.ValueObjects;
using MediatR;

namespace DanaWarga.Application.Features.Reports.Queries.GetPaymentMatrix;

public sealed class GetPaymentMatrixQueryHandler(
    IHouseRepository houseRepository,
    IIplPaymentRepository paymentRepository,
    IIplPriceConfigurationRepository priceRepository) : IRequestHandler<GetPaymentMatrixQuery, IplMatrixReportResult>
{
    public async Task<IplMatrixReportResult> Handle(GetPaymentMatrixQuery request, CancellationToken cancellationToken)
    {
        var houses = await houseRepository.ListAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var rows = new List<MatrixRowResult>();

        foreach (var house in houses)
        {
            var payments = await paymentRepository.ListByHouseAsync(house.Id, cancellationToken);
            var validAllocations = payments
                .Where(x => x.Status == PaymentStatus.Validated)
                .SelectMany(x => x.Allocations)
                .GroupBy(x => new Period(x.Year, x.Month))
                .ToDictionary(g => g.Key, g => g.Sum(x => x.AllocatedAmount.Amount));

            var allConfigs = await priceRepository.ListByHouseAsync(house.Id, cancellationToken);
            var pricingService = new DanaWarga.Domain.Services.IplPricingService();

            var monthCells = new List<MatrixMonthCellResult>(12);
            for (int month = 1; month <= 12; month++)
            {
                var key = new Period(request.Year, month);
                var required = pricingService.ResolvePrice(allConfigs, key, house.Id).Amount;
                var allocated = validAllocations.TryGetValue(key, out var val) ? val : 0m;

                var status = ResolveStatus(now, request.Year, month, required, allocated);
                monthCells.Add(new MatrixMonthCellResult(request.Year, month, ToResultStatus(status), required, allocated));
            }

            rows.Add(new MatrixRowResult(house.Id, house.HouseNumber, house.Resident?.FullName ?? string.Empty, monthCells));
        }

        var paidHouses = rows.Count(row => row.Months.Where(x => x.Month <= DateTime.UtcNow.Month).All(x => x.Status == MatrixStatusResult.Paid));
        var unpaidHouses = rows.Count - paidHouses;
        var totalExpected = rows.SelectMany(x => x.Months.Where(m => m.Year < now.Year || (m.Year == now.Year && m.Month <= now.Month))).Sum(x => x.RequiredAmount);
        var totalCollected = rows.SelectMany(x => x.Months).Sum(x => Math.Min(x.RequiredAmount, x.AllocatedAmount));
        var collectionRate = totalExpected == 0 ? 0 : decimal.Round((totalCollected / totalExpected) * 100m, 2);

        return new IplMatrixReportResult(request.Year, rows, paidHouses, unpaidHouses, collectionRate);
    }

    private static DanaWarga.Domain.Enums.PeriodPaymentState ResolveStatus(DateTime now, int year, int month, decimal required, decimal allocated)
    {
        var isFuture = year > now.Year || (year == now.Year && month > now.Month);
        if (allocated >= required)
        {
            return DanaWarga.Domain.Enums.PeriodPaymentState.Paid;
        }

        if (isFuture)
        {
            return DanaWarga.Domain.Enums.PeriodPaymentState.Empty;
        }

        if (allocated == 0)
        {
            return DanaWarga.Domain.Enums.PeriodPaymentState.Unpaid;
        }

        return DanaWarga.Domain.Enums.PeriodPaymentState.Empty;
    }

    private static MatrixStatusResult ToResultStatus(DanaWarga.Domain.Enums.PeriodPaymentState state)
    {
        return state switch
        {
            DanaWarga.Domain.Enums.PeriodPaymentState.Paid => MatrixStatusResult.Paid,
            DanaWarga.Domain.Enums.PeriodPaymentState.Unpaid => MatrixStatusResult.Unpaid,
            _ => MatrixStatusResult.Empty
        };
    }
}