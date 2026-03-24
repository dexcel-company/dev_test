using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Users.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Users.Commands.RefreshToken.Abstractions;

[ScopedHandler]
public interface IRefreshUserTokenCommandHandler
{
    Task<ErrorOr<UserToken>> HandleAsync(
        RefreshUserTokenCommand request, CancellationToken cancellationToken = default);
}
