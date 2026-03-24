using CelloPark.Application.Common.Tokens.Generators.Abstractions;
using System.Security.Cryptography;

namespace CelloPark.Infrastructure.Common.Tokens.Generators;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private const int ByteArraySize = 48;

    public string GenerateToken()
    {
        Span<byte> randomBytes = stackalloc byte[ByteArraySize];
        using RandomNumberGenerator generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }
}
