namespace DanaWarga.Contracts.Payments;

public sealed record SubmitIplPaymentRequest(Guid ResidentId, Guid HouseId, decimal TotalAmount, DateTime PaymentDate, string? ProofFilePath);