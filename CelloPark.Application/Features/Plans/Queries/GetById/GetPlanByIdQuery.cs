namespace CelloPark.Application.Features.Plans.Queries.GetById;

public sealed class GetPlanByIdQuery
{
    public GetPlanByIdQuery(Guid planId)
    {
        PlanId = planId;
    }

    public Guid PlanId { get; }
}
