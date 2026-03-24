using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Packets.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Packets.Queries.GetById.Abstractions;

[ScopedHandler]
public interface IGetPackageByIdQueryHandler
{
    Task<ErrorOr<PackageGetDto>> HandleAsync(
        GetPackageByIdQuery request, CancellationToken cancellationToken = default);
}
