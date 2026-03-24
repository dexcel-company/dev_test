namespace CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;

public sealed class CustomerCouponUsageGetDto
{
    public required Guid Id { get; init; }
    public required string Coupon { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
}
