using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Users.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Users.Commands.SignUp.Abstractions;

[ScopedHandler]
public interface ISignUpUserCommandHandler
{
    Task<ErrorOr<UserToken>> HandleAsync(
        SignUpUserCommand request, CancellationToken cancellationToken = default);
}
