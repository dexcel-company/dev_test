using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Benefits.Commands.ChangeStatus.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Commands.ChangeStatus;

internal sealed class ChangeBenefitStatusQueryHandler :
    IChangeBenefitStatusQueryHandler
{
    public ChangeBenefitStatusQueryHandler(IManagementContext managementContext)
    {
        _managementContext = managementContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        ChangeBenefitStatusQuery request, CancellationToken cancellationToken = default)
    {
        Benefit? benefit = await _managementContext.Benefits
            .Include(benefit => benefit.PaymentCategories)
            .Include(benefit => benefit.Coupons)
            .FirstOrDefaultAsync(benefit => benefit.Id == request.BenefitId, cancellationToken);

        if (benefit is null)
        {
            return BenefitErrors.NotFound;
        }

        switch (benefit.Status)
        {
            case Status.Active:
                benefit.MarkAsInactive();
                break;
            case Status.Inactive:
                benefit.MarkAsActive();
                break;
            case Status.Deleted:
                benefit.MarkAsInactive();
                break;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
