namespace CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;

public sealed class CustomerCouponUsageCreateDto
{
    public CustomerCouponUsageCreateDto(
        string coupon)
    {
        Coupon = coupon;
    }

    public string Coupon { get; }
}
