using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;

public sealed class BenefitCouponPageDto
{
    public required Guid Id { get; init; }
    public required string Coupon { get; init; }
    public required CouponType CouponType { get; init; }
    public required int Duration { get; init; }
    public required string Status { get; init; }
    public required bool IsUsed { get; init; }
    public required BenefitLiteDto Benefit { get; init; }
}
