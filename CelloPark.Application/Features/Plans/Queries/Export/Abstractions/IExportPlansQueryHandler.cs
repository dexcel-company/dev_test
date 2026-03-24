using CelloPark.Application.Common.Attributes;

namespace CelloPark.Application.Features.Plans.Queries.Export.Abstractions;

[ScopedHandler]
public interface IExportPlansQueryHandler
{
    Task<FileStream> HandleAsync(
        ExportPlansQuery request, CancellationToken cancellationToken = default);
}
