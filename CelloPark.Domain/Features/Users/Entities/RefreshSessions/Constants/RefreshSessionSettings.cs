namespace CelloPark.Domain.Features.Users.Entities.RefreshSessions.Constants;

public static class RefreshSessionSettings
{
    public const int RefreshTokenMinLength = 32;
    public const int RefreshTokenMaxLength = 64;

    public const int UserAgentMinLength = 8;
    public const int UserAgentMaxLength = 200;

    public const int FingerPrintMinLength = 8;
    public const int FingerPrintMaxLength = 200;

    public const int IpAddressMinLength = 8;
    public const int IpAddressMaxLength = 15;

    public const int ExpiresInDays = 30;
}
