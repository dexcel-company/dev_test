using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetAllPackages;

public sealed class GetAllCustomerPackagesQuery
{
    public GetAllCustomerPackagesQuery(
        Guid customerId,
        Guid customerPlanId,
        PaginationCriteria paginationCriteria)
    {
        CustomerId = customerId;
        CustomerPlanId = customerPlanId;
        PaginationCriteria = paginationCriteria;
    }

    public Guid CustomerId { get; }
    public Guid CustomerPlanId { get; }
    public PaginationCriteria PaginationCriteria { get; }
}
