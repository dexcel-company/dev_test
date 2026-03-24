namespace CelloPark.Application.Features.Users.Dtos;

public sealed class UserSignUpDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
