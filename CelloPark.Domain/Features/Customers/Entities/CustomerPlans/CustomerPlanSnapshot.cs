using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.Plans;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerPlans;

public sealed class CustomerPlanSnapshot
{
    public Guid Id { get; }
    public long PlanId { get; }
    public PlanSnapshot Plan { get; } = null!;
    public decimal Price { get; }
    public int Vat { get; }
    public DateOnly SnapshotDate { get; }
}
