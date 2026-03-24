namespace CelloPark.Application.Features.Users.Dtos;

public sealed class UserInformationDto
{
    public required Guid? UserId { get; init; }
    public required string? UserAgent { get; init; }
    public required string? Fingerprint { get; init; }
    public required string? IpAddress { get; init; }
}
