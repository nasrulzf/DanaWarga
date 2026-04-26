using System.Security.Cryptography;
using System.Text;
using DanaWarga.Application.Abstractions.Security;

namespace DanaWarga.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string hashedPassword, string providedPassword)
    {
        var providedHash = Hash(providedPassword);
        return string.Equals(hashedPassword, providedHash, StringComparison.Ordinal);
    }
}