using CelloPark.Application.Common.Attributes;

namespace CelloPark.Application.Features.Packets.Queries.Export.Abstractions;

[ScopedHandler]
public interface IExportPackagesQueryHandler
{
    Task<FileStream> HandleAsync(
        ExportPackagesQuery request, CancellationToken cancellationToken = default);
}
