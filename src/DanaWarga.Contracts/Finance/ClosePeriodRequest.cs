namespace DanaWarga.Contracts.Finance;

public sealed record ClosePeriodRequest(int Year, int Month, Guid ClosedBy);
