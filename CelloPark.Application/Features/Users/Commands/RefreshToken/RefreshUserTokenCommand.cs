namespace CelloPark.Application.Features.Users.Commands.RefreshToken;

public sealed class RefreshUserTokenCommand
{
    public RefreshUserTokenCommand(
        string refreshToken,
        string userAgent,
        string fingerprint,
        string ipAddress)
    {
        RefreshToken = refreshToken;
        UserAgent = userAgent;
        Fingerprint = fingerprint;
        IpAddress = ipAddress;
    }

    public string RefreshToken { get; }
    public string UserAgent { get; }
    public string Fingerprint { get; }
    public string IpAddress { get; }
}
