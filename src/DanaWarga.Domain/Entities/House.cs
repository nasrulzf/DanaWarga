using DanaWarga.Domain.Common;

namespace DanaWarga.Domain.Entities;

public sealed class House : Entity
{
    public string HouseNumber { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    public Guid ResidentId { get; private set; }
    public Resident? Resident { get; private set; }

    public ICollection<IplPriceConfiguration> PriceConfigurations { get; private set; } = new List<IplPriceConfiguration>();

    private House()
    {
    }

    public House(string houseNumber, string address, Guid residentId)
    {
        HouseNumber = houseNumber;
        Address = address;
        ResidentId = residentId;
    }
}