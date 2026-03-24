namespace CelloPark.Domain.Features.Users.Entities.RefreshSessions.Errors;

public static class RefreshSessionErrorCodes
{
    public const string RefreshToken = "RefreshSession.RefreshToken";
    public const string UserAgent = "RefreshSession.UserAgent";
    public const string Fingerprint = "RefreshSession.Fingerprint";
    public const string IpAddress = "RefreshSession.IpAddress";
    public const string CreatedAt = "RefreshSession.CreatedAt";
    public const string NotFound = "RefreshSession.NotFound";
}
