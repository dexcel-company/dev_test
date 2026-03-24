using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Benefits.Dtos;

public sealed class BenefitPageDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int Coupons { get; init; }
    public required string Applied { get; init; }
    public required DateTime? StartPrometionDate { get; init; }
    public required DateTime? EndPromotionDate { get; init; }
    public required IReadOnlyCollection<BenefitPaymentCategoryPageDto> PaymentCategories { get; init; }
    public required string Status { get; init; }
    public required DateTime? CreatedAt { get; init; }
    public required UserAuditDto? CreatedBy { get; init; }
}
