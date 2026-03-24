using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.Update.Abstractions;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Extensions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Errors;
using CelloPark.Domain.Features.Customers.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.Update;

internal sealed class UpdateCustomerPlanCommandHandler :
    IUpdateCustomerPlanCommandHandler
{
    public UpdateCustomerPlanCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        UpdateCustomerPlanCommand request, CancellationToken cancellationToken = default)
    {
        bool exists = await _managementContext.Customers
            .AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (!exists)
        {
            return CustomerErrors.NotFound;
        }

        CustomerPlan? customerPlan = await _managementContext.CustomerPlans
            .FirstOrDefaultAsync(x => x.Id == request.CustomerPlanId, cancellationToken: cancellationToken);

        if (customerPlan is null)
        {
            return CustomerPlanErrors.NotFound;
        }

        ErrorOr<CustomerPlan> customerPlanResult = customerPlan.Update(request.Dto);

        if (customerPlanResult.IsError)
        {
            return customerPlanResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
