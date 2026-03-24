using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Responses;
using ErrorOr;

namespace CelloPark.Application.Features.Packets.Commands.Create.Abstractions;

[ScopedHandler]
public interface ICreatePackageCommandHandler
{
    Task<ErrorOr<IdResult>> HandleAsync(
        CreatePackageCommand request, CancellationToken cancellationToken = default);
}
