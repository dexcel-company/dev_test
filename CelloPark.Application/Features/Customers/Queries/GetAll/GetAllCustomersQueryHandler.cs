using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Extensions;
using CelloPark.Application.Features.Customers.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Customers.Queries.GetAll;

internal sealed class GetAllCustomersQueryHandler :
    IGetAllCustomersQueryHandler
{
    public GetAllCustomersQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<Page<CustomerPageDto>> HandleAsync(
        GetAllCustomersQuery request, CancellationToken cancellationToken = default)
    {
        Page<CustomerPageDto> customerPage = await _managementContext.Customers
            .ApplyFiltering(request.FilteringCriteria)
            .ApplySorting(request.SortingCriteria)
            .Select(customer => new CustomerPageDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PlanName = customer.Plan.Plan.Name,
                PlanPrice = customer.Plan.Price == null
                    ? customer.Plan.Plan.Price
                    : customer.Plan.Price.Value,
                PackageCount = customer.Plan.PlanPackages.Count(),
                CarCount = customer.Cars.Count(),
                ContractType = customer.ContractType,
                CreatedAt = customer.CreateDetails.CreatedAt,
                CreatedBy = customer.CreateDetails.User == null ? null : new UserAuditDto
                {
                    Id = customer.CreateDetails.User.Id,
                    FirstName = customer.CreateDetails.User.FirstName,
                    LastName = customer.CreateDetails.User.LastName,
                }
            })
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return customerPage;
    }
}
