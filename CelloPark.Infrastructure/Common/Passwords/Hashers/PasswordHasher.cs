using BCrypt.Net;
using CelloPark.Application.Common.Passwords.Hashers.Abstractions;

namespace CelloPark.Infrastructure.Common.Passwords.Hashers;

internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly HashType _hashType = HashType.SHA384;
    private readonly bool _enhancedEntropy = true;

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return string.Empty;
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
            inputKey: password,
            salt: BCrypt.Net.BCrypt.GenerateSalt(),
            enhancedEntropy: _enhancedEntropy,
            hashType: _hashType);

        return hashedPassword;
    }

    public bool VerifyHashedPassword(string providedPassword, string hashedPassword)
    {
        if (string.IsNullOrEmpty(providedPassword) || string.IsNullOrEmpty(hashedPassword))
        {
            return false;
        }

        return BCrypt.Net.BCrypt.Verify(
            text: providedPassword,
            hash: hashedPassword,
            enhancedEntropy: _enhancedEntropy,
            hashType: _hashType);
    }
}
