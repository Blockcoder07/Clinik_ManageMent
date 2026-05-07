using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Clini_Management_System.Server.Helpers;

public static class PasswordHasher
{
    #region Constants

    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;
    private const char Separator = '.';
    private const KeyDerivationPrf Algorithm = KeyDerivationPrf.HMACSHA256;

    #endregion

    #region Public Methods

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = KeyDerivation.Pbkdf2(password, salt, Algorithm, Iterations, HashSize);
        return $"{Convert.ToBase64String(salt)}{Separator}{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string storedHash)
    {
        var parts = storedHash.Split(Separator);
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var expected = Convert.FromBase64String(parts[1]);
        var actual = KeyDerivation.Pbkdf2(password, salt, Algorithm, Iterations, HashSize);

        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }

    #endregion
}
