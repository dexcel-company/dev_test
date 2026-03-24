using CelloPark.Application.Common.Attributes;

namespace CelloPark.Application.Features.Benefits.Queries.Export.Abstractions;

[ScopedHandler]
public interface IExportBenefitsQueryHandler
{
    Task<FileStream> HandleAsync(
        ExportBenefitsQuery request, CancellationToken cancellationToken = default);
}
