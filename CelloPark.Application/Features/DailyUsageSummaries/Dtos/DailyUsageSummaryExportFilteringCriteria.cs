namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos;

public sealed class DailyUsageSummaryExportFilteringCriteria
{
    public DateOnly? CurrentStartDate { get; init; }
    public DateOnly? CurrentEndDate { get; init; }
    public DateOnly? ReferenceStartDate { get; init; }
    public DateOnly? ReferenceEndDate { get; init; }
}
