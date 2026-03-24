using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;

public sealed class BenefitPaymentCategoryCalculationDto
{
    public required Guid Id { get; init; }
    public required Guid? PlanId { get; init; }
    public required Guid? PackageId { get; init; }
    public required Guid? ItemId { get; init; }
    public required Guid BenefitId { get; init; }
    public required decimal Amount { get; init; }
    public required AmountType AmountType { get; init; }
    public required int? Frequency { get; init; }
    public required FrequencyType FrequencyType { get; init; }
    public required decimal? AmountLimit { get; init; }
}
