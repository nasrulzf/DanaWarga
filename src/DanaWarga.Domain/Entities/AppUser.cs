using DanaWarga.Domain.Common;

namespace DanaWarga.Domain.Entities;

public sealed class AppUser : Entity
{
    public string UserName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;

    public ICollection<AppUserRole> Roles { get; private set; } = new List<AppUserRole>();

    private AppUser()
    {
    }

    public AppUser(string userName, string passwordHash, string fullName)
    {
        UserName = userName;
        PasswordHash = passwordHash;
        FullName = fullName;
    }

    public void SetPasswordHash(string hash) => PasswordHash = hash;
}