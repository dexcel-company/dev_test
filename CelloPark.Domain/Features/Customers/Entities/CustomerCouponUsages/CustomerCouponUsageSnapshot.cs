using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Benefits;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;

public sealed class CustomerCouponUsageSnapshot
{
    public Guid Id { get; }
    public string Coupon { get; } = null!;
    public string CustomerId { get; } = null!;
    public Guid BenefitId { get; }
    public BenefitSnapshot Benefit { get; } = null!;
    public Status Status { get; }
    public DateOnly SnapshotDate { get; }
}
