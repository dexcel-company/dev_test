using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Users.Commands.SignIn;

public sealed class SignInUserCommand
{
    public SignInUserCommand(
        UserSignInDto dto,
        string userAgent,
        string fingerprint,
        string ipAddress)
    {
        Dto = dto;
        UserAgent = userAgent;
        Fingerprint = fingerprint;
        IpAddress = ipAddress;
    }

    public UserSignInDto Dto { get; }
    public string UserAgent { get; }
    public string Fingerprint { get; }
    public string IpAddress { get; }
}
