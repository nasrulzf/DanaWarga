using DanaWarga.Domain.ValueObjects;

namespace DanaWarga.Domain.Models;

public readonly record struct PeriodOutstanding(Period Period, Money OutstandingAmount);