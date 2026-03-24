using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyPlanCalculations;

public sealed class DailyPlanUsageCalculation :
    IStatusOwner
{
    private DailyPlanUsageCalculation() { }

    private DailyPlanUsageCalculation(
        string customerId,
        Guid? benefitId,
        decimal cost,
        int carCount)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        BenefitId = benefitId;
        Cost = cost;
        CarCount = carCount;
        Status = Status.Active;
    }

    public Guid Id { get; }
    public string CustomerId { get; private set; } = null!;
    public Guid CustomerPlanId { get; private set; }
    public Guid? BenefitId { get; private set; }
    public decimal Cost { get; private set; }
    public int CarCount { get; private set; }
    public Status Status { get; private set; }

    public static ErrorOr<DailyPlanUsageCalculation> Create(
        string customerId,
        Guid? benefitId,
        decimal cost,
        int carCount)
    {
        return new DailyPlanUsageCalculation(
            customerId: customerId,
            benefitId: benefitId,
            cost: cost,
            carCount: carCount);
    }

    public void MarkAsDeleted()
    {
        Status = Status.Deleted;
    }

    public void MarkAsActive()
    {
        Status = Status.Active;
    }

    public void MarkAsInactive()
    {
        Status = Status.Inactive;
    }
}
