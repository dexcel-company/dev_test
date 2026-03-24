namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;

public sealed class BenefitCouponUpdateDto
{
    public required string Coupon { get; init; }
    public required byte CouponType { get; init; }
}
