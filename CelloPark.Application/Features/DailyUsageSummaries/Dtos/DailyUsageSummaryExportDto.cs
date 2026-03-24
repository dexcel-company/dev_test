namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos;

public sealed class ExportDailyUsageDto
{
    public required string Name { get; init; }
    public required int Quantity { get; init; }
    public required decimal Gross { get; init; }
    public required int BenefitQuantity { get; init; }
    public required decimal BenefitCost { get; init; }
    public required decimal Cost { get; init; }
}
