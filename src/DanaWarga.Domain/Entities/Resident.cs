using DanaWarga.Domain.Common;

namespace DanaWarga.Domain.Entities;

public sealed class Resident : Entity
{
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public ICollection<House> Houses { get; private set; } = new List<House>();

    private Resident()
    {
    }

    public Resident(string fullName, string email, string phoneNumber)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}