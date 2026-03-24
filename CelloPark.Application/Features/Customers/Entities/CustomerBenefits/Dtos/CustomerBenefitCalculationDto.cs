namespace CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;

public sealed class CustomerBenefitCalculationDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public Guid BenefitId { get; init; }
    public int? FrequencyCountLeft { get; init; }
    public decimal? LimitAmountLeft { get; init; }
    public decimal? Debit { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
