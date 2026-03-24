namespace CelloPark.Application.Features.Customers.Dtos;

public sealed class CustomerFilteringQuery
{
    public string? Search { get; init; }
    public string? Coupon { get; init; }
}
