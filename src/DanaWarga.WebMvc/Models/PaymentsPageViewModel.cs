using DanaWarga.Contracts.Houses;
using DanaWarga.Contracts.Payments;
using DanaWarga.Contracts.Residents;

namespace DanaWarga.WebMvc.Models;

public sealed class PaymentsPageViewModel
{
    public IReadOnlyCollection<IplPaymentDto> Payments { get; set; } = [];
    public IReadOnlyCollection<ResidentDto> Residents { get; set; } = [];
    public IReadOnlyCollection<HouseDto> Houses { get; set; } = [];
}