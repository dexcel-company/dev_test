using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Users.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Users.Commands.SignIn.Abstractions;

[ScopedHandler]
public interface ISignInUserCommandHandler
{
    Task<ErrorOr<UserToken>> HandleAsync(
        SignInUserCommand request, CancellationToken cancellationToken = default);
}
