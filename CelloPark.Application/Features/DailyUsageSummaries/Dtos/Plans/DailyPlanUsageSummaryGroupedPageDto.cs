namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos.Plans;

public sealed class DailyPlanUsageSummaryGroupedPageDto
{
    public required DateOnly Date { get; init; }
    public required IReadOnlyCollection<DailyPlanUsageSummaryPageDto> Plans { get; init; }
}
