using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Users.Commands.SignOut.Abstractions;

[ScopedHandler]
public interface ISignOutUserCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        SignOutUserCommand request, CancellationToken cancellationToken = default);
}
