using DanaWarga.Contracts.Houses;
using DanaWarga.Contracts.Residents;

namespace DanaWarga.WebMvc.Models;

public sealed class HousesPageViewModel
{
    public IReadOnlyCollection<HouseDto> Houses { get; set; } = [];
    public IReadOnlyCollection<ResidentDto> Residents { get; set; } = [];
}