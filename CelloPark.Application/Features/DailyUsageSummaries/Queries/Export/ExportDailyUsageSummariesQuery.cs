using CelloPark.Application.Features.DailyUsageSummaries.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Queries.Export;

public sealed class ExportDailyUsageSummariesQuery
{
    public ExportDailyUsageSummariesQuery(
        DailyUsageSummaryExportFilteringCriteria filteringCriteria)
    {
        FilteringCriteria = filteringCriteria;
    }

    public DailyUsageSummaryExportFilteringCriteria FilteringCriteria { get; }
}
