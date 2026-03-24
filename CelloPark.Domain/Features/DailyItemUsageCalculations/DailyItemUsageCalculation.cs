using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyItemUsageCalculations;

public sealed class DailyItemUsageCalculation :
    IStatusOwner
{
    private DailyItemUsageCalculation() { }

    private DailyItemUsageCalculation(
        string customerId,
        Guid? benefitId,
        decimal cost)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        BenefitId = benefitId;
        Cost = cost;
        Status = Status.Active;
    }

    public Guid Id { get; }
    public string CustomerId { get; private set; } = null!;
    public Guid CustomerCarId { get; private set; }
    public Guid? BenefitId { get; private set; }
    public decimal Cost { get; private set; }
    public Status Status { get; private set; }

    public static ErrorOr<DailyItemUsageCalculation> Create(
        string customerId,
        Guid? benefitId,
        decimal cost)
    {
        return new DailyItemUsageCalculation(
            customerId: customerId,
            benefitId: benefitId,
            cost: cost);
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
