using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.Packets.Commands.SetPriceForPlan;

public sealed class SetPackagePriceForPlanCommand
{
    public SetPackagePriceForPlanCommand(
        Guid packageId,
        Guid planId,
        PackagePlanCreateDto dto)
    {
        PackageId = packageId;
        PlanId = planId;
        Dto = dto;
    }

    public Guid PackageId { get; }
    public Guid PlanId { get; }
    public PackagePlanCreateDto Dto { get; }
}
