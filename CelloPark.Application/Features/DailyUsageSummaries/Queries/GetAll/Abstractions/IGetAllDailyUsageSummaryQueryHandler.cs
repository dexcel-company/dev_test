using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllDailyUsageSummaryQueryHandler
{
    Task<DailyUsageSummaryPageDto> HandleAsync(
        GetAllDailyUsageSummaryQuery request, CancellationToken cancellationToken = default);
}
