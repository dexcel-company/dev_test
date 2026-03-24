namespace CelloPark.Application.Features.Users.Dtos;

public sealed class UserToken
{
    public string TokenType { get; } = "Bearer";
    public required string AccessToken { get; init; }
    public required long ExpiresIn { get; init; }
    public required string RefreshToken { get; init; }
}
