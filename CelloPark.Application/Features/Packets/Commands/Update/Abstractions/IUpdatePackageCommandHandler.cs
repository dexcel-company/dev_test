using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Packets.Commands.Update.Abstractions;

[ScopedHandler]
public interface IUpdatePackageCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        UpdatePackageCommand request, CancellationToken cancellationToken = default);
}
