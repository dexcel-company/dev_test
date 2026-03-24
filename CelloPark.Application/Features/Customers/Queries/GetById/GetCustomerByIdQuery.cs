namespace CelloPark.Application.Features.Customers.Queries.GetById;

public sealed class GetCustomerByIdQuery
{
    public GetCustomerByIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
}
