using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Benefits.Commands.Delete.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Commands.Delete;

internal sealed class DeleteBenefitCommandHandler :
    IDeleteBenefitCommandHandler
{
    public DeleteBenefitCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        DeleteBenefitCommand request, CancellationToken cancellationToken = default)
    {
        Benefit? benefit = await _managementContext.Benefits
            .Include(benefit => benefit.Coupons)
            .Include(benefit => benefit.PaymentCategories)
            .FirstOrDefaultAsync(benefit => benefit.Id == request.BenefitId, cancellationToken);

        if (benefit is null)
        {
            return BenefitErrors.NotFound;
        }

        benefit.MarkAsDeleted();
        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
