using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetById.Abstractions;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetById;

internal sealed class GetCustomerPlanByIdQueryHandler :
    IGetCustomerPlanByIdQueryHandler
{
    public GetCustomerPlanByIdQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<CustomerPlanGetDto>> HandleAsync(
        GetCustomerPlanByIdQuery request, CancellationToken cancellationToken = default)
    {
        CustomerPlanGetDto? customerPlanGetDto = await _managementContext.Customers
            .Where(customer => customer.Id == request.CustomerId)
            .Select(customer => customer.Plan == null ? null : new CustomerPlanGetDto
            {
                Id = customer.Plan.Id,
                Plan = new PlanLiteDto
                {
                    Id = customer.Plan.Plan.Id,
                    Name = customer.Plan.Plan.Name,
                },
                CustomerPackages = customer.Plan.PlanPackages.Select(CustomerPackage => new CustomerPackagePageDto
                {
                    Id = CustomerPackage.Id,
                    Package = new PackageLiteDto
                    {
                        Id = CustomerPackage.Package.Id,
                        Name = CustomerPackage.Package.Name,
                    },
                    Price = CustomerPackage.Price,
                    Vat = CustomerPackage.Vat,
                }).ToList(),
                Price = customer.Plan.Price == null
                    ? customer.Plan.Plan.Price
                    : customer.Plan.Price.Value,
                Vat = customer.Plan.Vat == null
                    ? customer.Plan.Plan.Vat
                    : customer.Plan.Vat.Value,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (customerPlanGetDto is null)
        {
            return CustomerPlanErrors.NotFound;
        }

        return customerPlanGetDto;
    }
}
