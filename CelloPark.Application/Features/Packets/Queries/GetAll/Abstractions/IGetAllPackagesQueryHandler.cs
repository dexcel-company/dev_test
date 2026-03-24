using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.Packets.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllPackagesQueryHandler
{
    Task<Page<PackagePageDto>> HandleAsync(
        GetAllPackagesQuery request, CancellationToken cancellationToken = default);
}
