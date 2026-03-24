namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;

public sealed class BenefitCouponCreateDto
{
    public required string Coupon { get; init; }
    public required byte CouponType { get; init; }
}
