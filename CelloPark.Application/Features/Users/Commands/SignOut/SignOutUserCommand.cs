namespace CelloPark.Application.Features.Users.Commands.SignOut;

public sealed class SignOutUserCommand
{
    public SignOutUserCommand(Guid userId, string refreshToken)
    {
        UserId = userId;
        RefreshToken = refreshToken;
    }

    public Guid UserId { get; }
    public string RefreshToken { get; }
}
