namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos.Packets;

public sealed class DailyPackageUsageSummaryGroupedPageDto
{
    public required DateOnly Date { get; init; }
    public required IReadOnlyCollection<DailyPackageUsageSummaryPageDto> Packages { get; init; }
}
