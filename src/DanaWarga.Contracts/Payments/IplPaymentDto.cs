namespace DanaWarga.Contracts.Payments;

public sealed record IplPaymentDto(
    Guid Id,
    Guid ResidentId,
    Guid HouseId,
    decimal TotalAmount,
    DateTime PaymentDate,
    string Status,
    IReadOnlyCollection<PaymentAllocationDto> Allocations);