using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Domain.Features.DailyItemUsageSummaries;

namespace CelloPark.Application.Features.DailyUsageSummaries.Extensions;

public static class DailyItemUsageSummaryExtensions
{
    public static IQueryable<DailyItemUsageSummary> ApplyFiltering(
        this IQueryable<DailyItemUsageSummary> source, DailyUsageSummaryFilteringQuery filteringCriteria)
    {
        if (filteringCriteria.StartDate is not null)
        {
            source = source
                .Where(dailyItemUsageSummary => dailyItemUsageSummary.Date >= filteringCriteria.StartDate);
        }

        if (filteringCriteria.EndDate is not null)
        {
            source = source
                .Where(dailyItemUsageSummary => dailyItemUsageSummary.Date <= filteringCriteria.EndDate);
        }

        return source;
    }

    public static IQueryable<DailyItemUsageSummary> ApplyFilteringByDates(
        this IQueryable<DailyItemUsageSummary> source, DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is not null)
        {
            source = source
                .Where(dailyItemUsageSummary => dailyItemUsageSummary.Date >= startDate);
        }

        if (endDate is not null)
        {
            source = source
                .Where(dailyItemUsageSummary => dailyItemUsageSummary.Date <= endDate);
        }

        return source;
    }
}
