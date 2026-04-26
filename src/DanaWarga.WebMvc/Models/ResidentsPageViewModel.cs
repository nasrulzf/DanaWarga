using DanaWarga.Contracts.Residents;

namespace DanaWarga.WebMvc.Models;

public sealed class ResidentsPageViewModel
{
    public IReadOnlyCollection<ResidentDto> Residents { get; set; } = [];
}