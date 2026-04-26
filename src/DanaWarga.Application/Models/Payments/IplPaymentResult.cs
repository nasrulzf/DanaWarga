namespace DanaWarga.Application.Models.Payments;

public sealed record IplPaymentResult(
    Guid Id,
    Guid ResidentId,
    Guid HouseId,
    decimal TotalAmount,
    DateTime PaymentDate,
    string Status,
    IReadOnlyCollection<PaymentAllocationResult> Allocations);