using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Benefits.Enums;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;

public sealed class BenefitCouponSnapshot
{
    private BenefitCouponSnapshot(
        Guid id,
        Guid benefitId,
        string coupon,
        CouponType couponType,
        int duration,
        Status status)
    {
        Id = id;
        BenefitId = benefitId;
        Coupon = coupon;
        CouponType = couponType;
        Duration = duration;
        Status = status;
    }

    public Guid Id { get; }
    public Guid BenefitId { get; private set; }
    public BenefitSnapshot Benefit { get; private set; } = null!;
    public string Coupon { get; private set; } = null!;
    public CouponType CouponType { get; private set; } = null!;
    public int Duration { get; private set; }
    public Status Status { get; private set; }
    public DateOnly SnapshotDate { get; set; }

    public static ErrorOr<BenefitCouponSnapshot> Create(
        Guid id,
        Guid benefitId,
        string coupon,
        CouponType couponType,
        int duration,
        Status status)
    {
        return new BenefitCouponSnapshot(
            id: id,
            benefitId: benefitId,
            coupon: coupon,
            couponType: couponType,
            duration: duration,
            status: status);
    }
}
