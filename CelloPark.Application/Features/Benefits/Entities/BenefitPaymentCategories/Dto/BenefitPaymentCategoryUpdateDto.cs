namespace CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;

public sealed class BenefitPaymentCategoryUpdateDto
{
    public required Guid? Plan { get; init; }
    public required Guid? Package { get; init; }
    public required Guid? Item { get; init; }
    public required decimal Amount { get; init; }
    public required byte AmountType { get; init; }
    public required decimal? AmountLimit { get; init; }
    public required int? Frequency { get; init; }
    public required byte FrequencyType { get; init; }
}
