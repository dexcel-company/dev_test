using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Features.Plans.Commands.Create.Abstractions;
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

namespace CelloPark.Application.Features.Plans.Commands.Create;

internal sealed class CreatePlanCommandHandler :
    ICreatePlanCommandHandler
{
    public CreatePlanCommandHandler(IManagementContext managementContext)
    {
        _managementContext = managementContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<IdResult>> HandleAsync(
        CreatePlanCommand request, CancellationToken cancellationToken = default)
    {
        CreatePlanCommandValidator validator = new();
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

        ErrorOr<Plan> planResult = request.Dto.ToModel();

        if (planResult.IsError)
        {
            return planResult.Errors;
        }

        await _managementContext.Plans.AddAsync(planResult.Value, cancellationToken);
        await _managementContext.SaveChangesAsync(cancellationToken);

        return new IdResult(planResult.Value.Id);
    }

    private async Task<ErrorOr<None>> ValidateRequestAsync(
        CreatePlanCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _managementContext.Plans
            .AnyAsync(plan => plan.Name == request.Dto.Name, cancellationToken);

        if (exists)
        {
            return PlanErrors.NameAlreadyExists;
        }

        if (request.Dto.ShadowId is not null)
        {
            exists = await _managementContext.Plans
                .AnyAsync(plan => plan.ShadowId == request.Dto.ShadowId, cancellationToken);

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
