using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Plans.Commands.Update;

public sealed class UpdatePlanCommand
{
    public UpdatePlanCommand(
        Guid planId,
        PlanUpdateDto dto)
    {
        PlanId = planId;
        Dto = dto;
    }

    public Guid PlanId { get; }
    public PlanUpdateDto Dto { get; }
}
