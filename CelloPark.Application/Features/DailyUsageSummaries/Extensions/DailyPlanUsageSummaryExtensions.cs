using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Domain.Features.DailyPlanUsageSummaries;

namespace CelloPark.Application.Features.DailyUsageSummaries.Extensions;

public static class DailyPlanUsageSummaryExtensions
{
    public static IQueryable<DailyPlanUsageSummary> ApplyFiltering(
        this IQueryable<DailyPlanUsageSummary> source, DailyUsageSummaryFilteringQuery filteringCriteria)
    {
        if (filteringCriteria.StartDate is not null)
        {
            source = source
                .Where(dailyPlanUsageSummary => dailyPlanUsageSummary.Date >= filteringCriteria.StartDate);
        }

        if (filteringCriteria.EndDate is not null)
        {
            source = source
                .Where(dailyPlanUsageSummary => dailyPlanUsageSummary.Date <= filteringCriteria.EndDate);
        }

        return source;
    }

    public static IQueryable<DailyPlanUsageSummary> ApplyFilteringByDates(
        this IQueryable<DailyPlanUsageSummary> source, DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is not null)
        {
            source = source
                .Where(dailyPlanUsageSummary => dailyPlanUsageSummary.Date >= startDate);
        }

        if (endDate is not null)
        {
            source = source
                .Where(dailyPlanUsageSummary => dailyPlanUsageSummary.Date <= endDate);
        }

        return source;
    }
}
