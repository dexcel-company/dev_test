using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;

namespace CelloPark.Application.Features.Benefits.Dtos;

public sealed class BenefitUpdateDto
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public Guid? Plan { get; init; }
    public Guid? Package { get; init; }
    public Guid? Item { get; init; }
    public DateTime? StartActiveDate { get; init; }
    public DateTime? EndActiveDate { get; init; }
    public DateTime? StartPromotionDate { get; init; }
    public DateTime? EndPromotionDate { get; init; }
    public int? ActivationDateDuration { get; init; }
    public int? CouponDateDuration { get; init; }
    public IReadOnlyCollection<BenefitCouponUpdateDto> Coupons { get; init; } = null!;
    public IReadOnlyCollection<BenefitPaymentCategoryUpdateDto> PaymentCategories { get; init; } = null!;
}
