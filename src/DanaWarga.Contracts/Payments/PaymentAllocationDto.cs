namespace DanaWarga.Contracts.Payments;

public sealed record PaymentAllocationDto(int Year, int Month, decimal AllocatedAmount);