namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos;

public sealed class DailyUsageSummaryCalculateDto
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
