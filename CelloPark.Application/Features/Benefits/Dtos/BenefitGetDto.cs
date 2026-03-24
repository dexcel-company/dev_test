using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Benefits.Dtos;

public sealed class BenefitGetDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required DateTime? StartActiveDate { get; init; }
    public required DateTime? EndActiveDate { get; init; }
    public required DateTime? StartPromotionDate { get; init; }
    public required DateTime? EndPromotionDate { get; init; }
    public required int? ActivationDateDuration { get; init; }
    public required int? CouponDateDuration { get; init; }
    public required IReadOnlyCollection<BenefitCouponPageDto> Coupons { get; init; }
    public required IReadOnlyCollection<BenefitPaymentCategoryPageDto> PaymentCategories { get; init; }
    public required DateTime? CreatedAt { get; init; }
    public required UserAuditDto? CreatedBy { get; init; }
}
