namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos.Items;

public sealed class DailyItemUsageSummaryGroupedPageDto
{
    public required DateOnly Date { get; init; }
    public required IReadOnlyCollection<DailyItemUsageSummaryPageDto> Items { get; init; }
}
