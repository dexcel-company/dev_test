namespace CelloPark.Application.Features.Plans.Commands.Delete;

public sealed class DeletePlanCommand
{
    public DeletePlanCommand(Guid planId)
    {
        PlanId = planId;
    }

    public Guid PlanId { get; }
}
