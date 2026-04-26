using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Domain.Entities;
using DanaWarga.Domain.Enums;
using DanaWarga.Domain.Models;
using DanaWarga.Domain.ValueObjects;
using BillingPeriod = DanaWarga.Domain.ValueObjects.Period;
using MediatR;

namespace DanaWarga.Application.Features.IplPayments.Commands.CreatePayment;

public sealed class CreatePaymentCommandHandler(
    IIplPaymentRepository paymentRepository,
    IHouseRepository houseRepository,
    IResidentRepository residentRepository,
    IPeriodRepository periodRepository,
    IFinancialPeriodRepository financialPeriodRepository,
    IIplPriceConfigurationRepository priceRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreatePaymentCommand, Guid>
{
    public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        _ = await residentRepository.GetByIdAsync(request.ResidentId, cancellationToken)
            ?? throw new InvalidOperationException("Resident not found.");

        _ = await houseRepository.GetByIdAsync(request.HouseId, cancellationToken)
            ?? throw new InvalidOperationException("House not found.");

        var payment = new IplPayment(
            request.ResidentId,
            request.HouseId,
            new Money(request.Amount),
            request.PaymentDate,
            request.ProofFilePath);

        var periods = await periodRepository.ListByHouseAsync(request.HouseId, cancellationToken);
        var housePayments = await paymentRepository.ListByHouseAsync(request.HouseId, cancellationToken);
        var allConfigs = await priceRepository.ListByHouseAsync(request.HouseId, cancellationToken);
        var pricingService = new DanaWarga.Domain.Services.IplPricingService();

        var allocatedByPeriod = housePayments
            .Where(x => x.Status == PaymentStatus.Validated)
            .SelectMany(x => x.Allocations)
            .GroupBy(x => new BillingPeriod(x.Year, x.Month))
            .ToDictionary(g => g.Key, g => g.Sum(x => x.AllocatedAmount.Amount));

        var orderedPeriods = periods
            .Select(x => new BillingPeriod(x.Year, x.Month))
            .OrderBy(x => x)
            .ToList();

        var remaining = request.Amount;
        var periodIndex = 0;
        var maxKnownPeriod = orderedPeriods.Count > 0
            ? orderedPeriods[^1]
            : new BillingPeriod(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        var scanGuard = 0;

        var outstandingPeriods = new List<PeriodOutstanding>();

        while (remaining > 0 && scanGuard < 240)
        {
            scanGuard++;

            BillingPeriod cursor;
            if (periodIndex < orderedPeriods.Count)
            {
                cursor = orderedPeriods[periodIndex];
                periodIndex++;
            }
            else
            {
                maxKnownPeriod = maxKnownPeriod.NextMonth();
                cursor = maxKnownPeriod;
            }

            if (await financialPeriodRepository.IsClosedAsync(cursor.Year, cursor.Month, cancellationToken))
            {
                continue;
            }

            var required = pricingService.ResolvePrice(allConfigs, cursor, request.HouseId).Amount;
            var paid = allocatedByPeriod.TryGetValue(cursor, out var value) ? value : 0m;
            var outstanding = Math.Max(0m, required - paid);

            if (outstanding > 0)
            {
                outstandingPeriods.Add(new PeriodOutstanding(cursor, new Money(outstanding)));
                remaining -= Math.Min(remaining, outstanding);
            }
        }

        if (remaining > 0)
        {
            throw new InvalidOperationException("Unable to allocate payment to open financial periods.");
        }

        payment.AllocatePayment(outstandingPeriods);
        await paymentRepository.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}