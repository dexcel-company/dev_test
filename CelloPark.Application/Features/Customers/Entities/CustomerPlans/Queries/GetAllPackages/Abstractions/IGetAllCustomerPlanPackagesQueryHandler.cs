using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetAllPackages.Abstractions;

[ScopedHandler]
public interface IGetAllCustomerPackagesQueryHandler
{
    Task<ErrorOr<Page<CustomerPackagePageDto>>> HandleAsync(
        GetAllCustomerPackagesQuery request, CancellationToken cancellationToken = default);
}
