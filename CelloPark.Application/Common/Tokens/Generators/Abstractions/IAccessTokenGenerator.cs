namespace CelloPark.Application.Common.Tokens.Generators.Abstractions;

public interface IAccessTokenGenerator
{
    (string, long) GenerateToken(Guid userId, string? firstName, string? lastname, DateTimeOffset utcNow);
}
