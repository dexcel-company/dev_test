namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;

public sealed class CustomerPlanUpdateDto
{
    public decimal Price { get; init; }
    public bool HasVat { get; init; }
}
