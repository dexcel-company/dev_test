using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetById.Abstractions;

[ScopedHandler]
public interface IGetCustomerPlanByIdQueryHandler
{
    Task<ErrorOr<CustomerPlanGetDto>> HandleAsync(
        GetCustomerPlanByIdQuery request, CancellationToken cancellationToken = default);
}
