using CelloPark.Application.Features.DailyUsageSummaries.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Queries.GetAll;

public sealed class GetAllDailyUsageSummaryQuery
{
    public GetAllDailyUsageSummaryQuery(
        DailyUsageSummaryFilteringQuery filteringCriteria)
    {
        FilteringCriteria = filteringCriteria;
    }

    public DailyUsageSummaryFilteringQuery FilteringCriteria { get; }
}
