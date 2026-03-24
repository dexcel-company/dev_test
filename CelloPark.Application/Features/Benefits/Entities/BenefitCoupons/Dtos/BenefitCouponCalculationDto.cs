using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;

public sealed class BenefitCouponCalculationDto
{
    public required Guid Id { get; init; }
    public required Guid BenefitId { get; init; }
    public required string Coupon { get; init; }
    public required CouponType CouponType { get; init; }
    public required int Duration { get; init; }
    public required Status Status { get; init; }
}
