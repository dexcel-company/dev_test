using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos.Plans;

public sealed class DailyPlanUsageSummaryPageDto
{
    public required PlanLiteDto Plan { get; init; }
    public required decimal Gross { get; init; }
    public required decimal Cost { get; init; }
    public required int Quantity { get; init; }
    public required decimal BenefitCost { get; init; }
    public required int BenefitQuantity { get; init; }
}
