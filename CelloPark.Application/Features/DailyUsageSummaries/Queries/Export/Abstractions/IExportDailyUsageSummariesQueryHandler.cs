using CelloPark.Application.Common.Attributes;

namespace CelloPark.Application.Features.DailyUsageSummaries.Queries.Export.Abstractions;

[ScopedHandler]
public interface IExportDailyUsageSummariesQueryHandler
{
    Task<FileStream> HandleAsync(
        ExportDailyUsageSummariesQuery request, CancellationToken cancellationToken = default);
}
