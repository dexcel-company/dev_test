namespace CelloPark.Application.Features.Users.Dtos;

public sealed class UserAuditDto
{
    public required Guid Id { get; init; }
    public required string? FirstName { get; init; }
    public required string? LastName { get; init; }
}
