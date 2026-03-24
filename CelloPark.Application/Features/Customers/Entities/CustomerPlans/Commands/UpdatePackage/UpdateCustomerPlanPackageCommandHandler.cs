using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.UpdatePackage.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Constants;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Errors;
using CelloPark.Domain.Features.Customers.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.UpdatePackage;

internal sealed class UpdateCustomerPackageCommandHandler :
    IUpdateCustomerPackageCommandHandler
{
    public UpdateCustomerPackageCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        UpdateCustomerPackageCommand request, CancellationToken cancellationToken = default)
    {
        bool exists = await _managementContext.Customers
            .AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (!exists)
        {
            return CustomerErrors.NotFound;
        }

        CustomerPlan? customerPlan = await _managementContext.CustomerPlans
            .Include(x => x.PlanPackages)
            .FirstOrDefaultAsync(x => x.Id == request.CustomerPlanId, cancellationToken: cancellationToken);

        if (customerPlan is null)
        {
            return CustomerPlanErrors.NotFound;
        }

        ErrorOr<None> customerPlanResult = customerPlan.UpdatePlanPackage(
            CustomerPackageId: request.CustomerPackageId,
            price: request.Dto.Price,
            vat: request.Dto.HasVat ? CustomerPlanSettings.VatDefaultValue : CustomerPlanSettings.VatMinValue);

        if (customerPlanResult.IsError)
        {
            return customerPlanResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
