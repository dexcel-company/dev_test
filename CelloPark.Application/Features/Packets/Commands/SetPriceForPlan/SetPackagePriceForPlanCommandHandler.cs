using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Packets.Commands.SetPriceForPlan.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages.Constants;
using CelloPark.Domain.Features.Packages.Errors;
using CelloPark.Domain.Features.Plans.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Packets.Commands.SetPriceForPlan;

internal sealed class SetPackagePriceForPlanCommandHandler :
    ISetPackagePriceForPlanCommandHandler
{
    public SetPackagePriceForPlanCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        SetPackagePriceForPlanCommand request, CancellationToken cancellationToken = default)
    {
        SetPackagePriceForPlanCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));
        }

        bool exists = await _managementContext.Packages
            .AnyAsync(x => x.Id == request.PackageId, cancellationToken);

        if (!exists)
        {
            return PackageErrors.NotFound;
        }

        exists = await _managementContext.Plans
            .AnyAsync(x => x.Id == request.PlanId, cancellationToken);

        if (!exists)
        {
            return PlanErrors.NotFound;
        }

        PlanPackage? planPackage = await _managementContext.PlanPackages
            .FirstOrDefaultAsync(x => x.PackageId == request.PackageId && x.PlanId == request.PlanId, cancellationToken);

        if (planPackage is null)
        {
            ErrorOr<PlanPackage> planPackageResult = PlanPackage.Create(
                planId: request.PlanId,
                packageId: request.PackageId,
                price: request.Dto.Price,
                vat: request.Dto.HasVat ? PlanPackageSettings.VatDefaultValue : PlanPackageSettings.VatMinValue);

            if (planPackageResult.IsError)
            {
                return planPackageResult.Errors;
            }

            await _managementContext.PlanPackages.AddAsync(planPackageResult.Value, cancellationToken);
        }
        else
        {
            ErrorOr<None> priceResult = planPackage.UpdatePrice(request.Dto.Price);

            if (priceResult.IsError)
            {
                return priceResult.FirstError;
            }

            ErrorOr<None> vatResult = planPackage.UpdateVat(request.Dto.HasVat ? PlanPackageSettings.VatDefaultValue : PlanPackageSettings.VatMinValue);

            if (vatResult.IsError)
            {
                return vatResult.FirstError;
            }
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
