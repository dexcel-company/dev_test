using CelloPark.Application.Features.Items.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos.Items;

public sealed class DailyItemUsageSummaryPageDto
{
    public required ItemLiteDto Item { get; init; }
    public required decimal Gross { get; init; }
    public required decimal Cost { get; init; }
    public required decimal BenefitCost { get; init; }
    public required int BenefitQuantity { get; init; }
    public required int Quantity { get; init; }
}
