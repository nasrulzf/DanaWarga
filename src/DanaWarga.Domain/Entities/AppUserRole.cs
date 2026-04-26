namespace DanaWarga.Domain.Entities;

public sealed class AppUserRole
{
    public Guid AppUserId { get; private set; }
    public AppUser? AppUser { get; private set; }

    public string RoleName { get; private set; } = string.Empty;

    private AppUserRole()
    {
    }

    public AppUserRole(Guid appUserId, string roleName)
    {
        AppUserId = appUserId;
        RoleName = roleName;
    }
}