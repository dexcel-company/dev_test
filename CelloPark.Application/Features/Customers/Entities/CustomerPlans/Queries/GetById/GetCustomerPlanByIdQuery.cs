namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetById;

public sealed class GetCustomerPlanByIdQuery
{
    public GetCustomerPlanByIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
}
