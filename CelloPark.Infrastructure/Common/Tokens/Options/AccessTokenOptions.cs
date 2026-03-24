namespace CelloPark.Infrastructure.Common.Tokens.Options;

internal class AccessTokenOptions
{
    public string Secret { get; init; } = null!;
    public long ExpiresIn { get; init; }
}
