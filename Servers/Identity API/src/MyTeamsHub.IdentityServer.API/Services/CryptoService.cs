using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace MyTeamsHub.IdentityServer.API.Services;

public class CryptoService : ICryptoService
{
    private const int ITERATION_COUNT = 35000;
    private const int BYTES_REQUESTED = 64;

    public string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(64);
        var hash = KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            ITERATION_COUNT,
            BYTES_REQUESTED);

        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string passwordHashBase64, string salt)
    {
        var passwordHash = KeyDerivation.Pbkdf2(
            password,
            Convert.FromBase64String(salt),
            KeyDerivationPrf.HMACSHA256,
            ITERATION_COUNT,
            BYTES_REQUESTED);

        var existingPasswordHash = Convert.FromBase64String(passwordHashBase64);

        return passwordHash.SequenceEqual(existingPasswordHash);
    }
}
