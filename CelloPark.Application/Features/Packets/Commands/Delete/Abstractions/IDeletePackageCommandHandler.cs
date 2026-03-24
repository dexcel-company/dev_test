using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Packets.Commands.Delete.Abstractions;

[ScopedHandler]
public interface IDeletePackageCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        DeletePackageCommand command, CancellationToken cancellationToken = default);
}
