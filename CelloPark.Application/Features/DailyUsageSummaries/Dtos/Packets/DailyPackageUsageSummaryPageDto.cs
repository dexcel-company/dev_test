using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Dtos.Packets;

public sealed class DailyPackageUsageSummaryPageDto
{
    public required PackageLiteDto Package { get; init; }
    public required decimal Gross { get; init; }
    public required decimal Cost { get; init; }
    public required decimal BenefitCost { get; init; }
    public required int BenefitQuantity { get; init; }
    public required int Quantity { get; init; }
}
