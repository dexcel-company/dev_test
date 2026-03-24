namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos;

public sealed class DailyUsageSummaryFilteringQuery
{
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}
