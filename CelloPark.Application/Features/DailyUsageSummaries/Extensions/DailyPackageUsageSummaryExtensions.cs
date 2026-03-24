using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Domain.Features.DailyPackageUsageSummaries;

namespace CelloPark.Application.Features.DailyUsageSummaries.Extensions;

public static class DailyPackageUsageSummaryExtensions
{
    public static IQueryable<DailyPackageUsageSummary> ApplyFiltering(
        this IQueryable<DailyPackageUsageSummary> source, DailyUsageSummaryFilteringQuery filteringCriteria)
    {
        if (filteringCriteria.StartDate is not null)
        {
            source = source
                .Where(dailyPackageUsageSummary => dailyPackageUsageSummary.Date >= filteringCriteria.StartDate);
        }

        if (filteringCriteria.EndDate is not null)
        {
            source = source
                .Where(dailyPackageUsageSummary => dailyPackageUsageSummary.Date <= filteringCriteria.EndDate);
        }

        return source;
    }

    public static IQueryable<DailyPackageUsageSummary> ApplyFilteringByDates(
        this IQueryable<DailyPackageUsageSummary> source, DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is not null)
        {
            source = source
                .Where(dailyPackageUsageSummary => dailyPackageUsageSummary.Date >= startDate);
        }

        if (endDate is not null)
        {
            source = source
                .Where(dailyPackageUsageSummary => dailyPackageUsageSummary.Date <= endDate);
        }

        return source;
    }
}
