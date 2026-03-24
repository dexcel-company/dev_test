using CelloPark.Application.Features.DailyUsageSummaries.Dtos.Items;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos.Packets;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos.Plans;

namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos;

public sealed class DailyUsageSummaryPageDto
{
    public required IReadOnlyCollection<DailyPlanUsageSummaryGroupedPageDto> PlanSummaries { get; init; }
    public required IReadOnlyCollection<DailyPackageUsageSummaryGroupedPageDto> PackageSummaries { get; init; }
    public required IReadOnlyCollection<DailyItemUsageSummaryGroupedPageDto> ItemSummaries { get; init; }
}
