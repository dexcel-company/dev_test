using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Users.Commands.SignUp;

public sealed class SignUpUserCommand
{
    public SignUpUserCommand(
        UserSignUpDto dto,
        string userAgent,
        string fingerprint,
        string ipAddress)
    {
        Dto = dto;
        UserAgent = userAgent;
        Fingerprint = fingerprint;
        IpAddress = ipAddress;
    }

    public UserSignUpDto Dto { get; }
    public string UserAgent { get; }
    public string Fingerprint { get; }
    public string IpAddress { get; }
}
