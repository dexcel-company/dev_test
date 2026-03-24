using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;

namespace CelloPark.Application.Features.Benefits.Dtos;

public sealed class BenefitCalculationDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required DateTime? StartActiveDate { get; init; }
    public required DateTime? EndActiveDate { get; init; }
    public required DateTime? StartPromotionDate { get; init; }
    public required DateTime? EndPromotionDate { get; init; }
    public required int? Duration { get; init; }
    public required int? CouponsDuration { get; init; }
    public required IEnumerable<BenefitPaymentCategoryCalculationDto> PaymentCategories { get; init; }
    public required IEnumerable<BenefitCouponCalculationDto> Coupons { get; init; }
}
