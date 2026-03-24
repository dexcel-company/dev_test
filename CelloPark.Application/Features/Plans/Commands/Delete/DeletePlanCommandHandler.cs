using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Plans.Commands.Delete.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Plans.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Plans.Commands.Delete;

internal sealed class DeletePlanCommandHandler :
    IDeletePlanCommandHandler
{
    public DeletePlanCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        DeletePlanCommand request, CancellationToken cancellationToken = default)
    {
        Plan? plan = await _managementContext.Plans
            .FirstOrDefaultAsync(x => x.Id == request.PlanId, cancellationToken);

        if (plan is null)
        {
            return PlanErrors.NotFound;
        }

        plan.MarkAsDeleted();
        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
