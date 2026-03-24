using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Queries.Create;

public sealed class CreateCustomerCoupoUsageQuery
{
    public CreateCustomerCoupoUsageQuery(
        string customerId,
        CustomerCouponUsageCreateDto dto)
    {
        CustomerId = customerId;
        Dto = dto;
    }

    public string CustomerId { get; }
    public CustomerCouponUsageCreateDto Dto { get; }
}
