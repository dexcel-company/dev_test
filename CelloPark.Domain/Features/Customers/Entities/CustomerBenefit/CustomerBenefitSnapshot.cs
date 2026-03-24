using CelloPark.Domain.Features.Benefits;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;

public sealed class CustomerBenefitSnapshot
{
    public Guid Id { get; }
    public string CustomerId { get; } = null!;
    public Guid BenefitId { get; }
    public BenefitSnapshot Benefit { get; } = null!;
    public decimal? Debit { get; }
    public int? FrequencyCountLeft { get; }
    public decimal? LimitAmountLeft { get; }
    public DateOnly SnapshotDate { get; }
}
