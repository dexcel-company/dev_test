using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyPackageUsageCalculations;

public sealed class DailyPackageUsageCalculation :
    IStatusOwner
{
    private DailyPackageUsageCalculation() { }

    private DailyPackageUsageCalculation(
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
    public Guid? BenefitId { get; private set; }
    public decimal Cost { get; private set; }
    public Status Status { get; private set; }

    public static ErrorOr<DailyPackageUsageCalculation> Create(
        string customerId,
        Guid? benefitId,
        decimal cost)
    {
        return new DailyPackageUsageCalculation(
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
