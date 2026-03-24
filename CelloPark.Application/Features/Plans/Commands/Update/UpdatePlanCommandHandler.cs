using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Plans.Commands.Update.Abstractions;
using CelloPark.Application.Features.Plans.Extensions;
using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.CalculationTypes.Errors;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Enums.ContractTypes.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Plans.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Plans.Commands.Update;

internal sealed class UpdatePlanCommandHandler :
    IUpdatePlanCommandHandler
{
    public UpdatePlanCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        UpdatePlanCommand request, CancellationToken cancellationToken = default)
    {
        UpdatePlanCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));
        }

        ErrorOr<None> requestResult = await ValidateRequestAsync(request, cancellationToken);

        if (requestResult.IsError)
        {
            return requestResult.FirstError;
        }

        Plan? plan = await _managementContext.Plans
            .FirstOrDefaultAsync(plan => plan.Id == request.PlanId, cancellationToken);

        if (plan is null)
        {
            return PlanErrors.NotFound;
        }

        ErrorOr<Plan> planResult = plan.Update(request.Dto);

        if (planResult.IsError)
        {
            return planResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }

    private async Task<ErrorOr<None>> ValidateRequestAsync(
        UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _managementContext.Plans
            .AnyAsync(plan => plan.Name == request.Dto.Name && plan.Id != request.PlanId, cancellationToken);

        if (exists)
        {
            return PlanErrors.NameAlreadyExists;
        }

        if (request.Dto.ShadowId is not null)
        {
            exists = await _managementContext.Plans
                .AnyAsync(plan => plan.ShadowId == request.Dto.ShadowId && plan.Id != request.PlanId, cancellationToken);

            if (exists)
            {
                return PlanErrors.ShadowIdAlreadyExists;
            }
        }

        ContractType? contractType = ContractType.FromKey(request.Dto.ContractType);

        if (contractType is null)
        {
            return ContractTypeErrors.NotFound;
        }

        CalculationType? calculationType = CalculationType.FromKey(request.Dto.CalculationType);

        if (calculationType is null)
        {
            return CalculationTypeErrors.NotFound;
        }

        if (contractType != ContractType.Business && calculationType != CalculationType.None)
        {
            return Error.Validation("CalculationType.Id", "Plan cannot contains calculation type if contract type not equals to 'Business'.");
        }

        return None.Value;
    }
}
