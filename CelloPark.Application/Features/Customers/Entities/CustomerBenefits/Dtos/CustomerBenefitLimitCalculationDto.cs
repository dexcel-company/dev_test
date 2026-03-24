namespace CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;

public sealed class CustomerBenefitLimitCalculationDto
{
    public Guid Id { get; init; }
    public int FrequencyCountLeft { get; init; }
    public decimal? LimitAmountLeft { get; init; }
}
