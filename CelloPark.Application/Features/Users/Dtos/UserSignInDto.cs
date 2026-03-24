namespace CelloPark.Application.Features.Users.Dtos;

public sealed class UserSignInDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
