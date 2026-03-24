using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetAllPackages.Abstractions;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Errors;
using CelloPark.Domain.Features.Customers.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetAllPackages;

internal sealed class GetAllCustomerPackagesQueryHandler :
    IGetAllCustomerPackagesQueryHandler
{
    public GetAllCustomerPackagesQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<Page<CustomerPackagePageDto>>> HandleAsync(
        GetAllCustomerPackagesQuery request, CancellationToken cancellationToken = default)
    {
        // TODO get all date througth the customer
        bool exists = await _managementContext.Customers
            .AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (!exists)
        {
            return CustomerErrors.NotFound;
        }

        exists = await _managementContext.CustomerPlans
            .AnyAsync(x => x.Id == request.CustomerPlanId, cancellationToken);

        if (!exists)
        {
            return CustomerPlanErrors.NotFound;
        }

        Page<CustomerPackagePageDto> CustomerPackagePage = await _managementContext.CustomerPlans
            .Where(customerPlan => customerPlan.Id == request.CustomerPlanId)
            .OrderBy(customerPlan => customerPlan.Id)
            .SelectMany(customerPlan => customerPlan.PlanPackages
                .Select(CustomerPackage => new CustomerPackagePageDto
                {
                    Id = CustomerPackage.Id,
                    Package = new PackageLiteDto
                    {
                        Id = CustomerPackage.Package.Id,
                        Name = CustomerPackage.Package.Name,
                    },
                    Price = CustomerPackage.Price,
                    Vat = CustomerPackage.Vat,
                }))
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return CustomerPackagePage;
    }
}
