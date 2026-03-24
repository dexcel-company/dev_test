namespace CelloPark.Application.Features.Users.ActionContexts.Abstractions;

public interface IUserActionContext
{
    string? AccessToken { get; }
    Guid? UserId { get; }
    string? UserAgent { get; }
    string? Fingerprint { get; }
    string? IpAddress { get; }
}
