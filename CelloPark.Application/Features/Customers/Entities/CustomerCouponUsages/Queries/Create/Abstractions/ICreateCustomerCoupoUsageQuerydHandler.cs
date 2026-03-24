using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Queries.Create.Abstractions;

[ScopedHandler]
public interface ICreateCustomerCoupoUsageQuerydHandler
{
    Task<ErrorOr<None>> HandleAsync(
        CreateCustomerCoupoUsageQuery request, CancellationToken cancellationToken = default);
}
