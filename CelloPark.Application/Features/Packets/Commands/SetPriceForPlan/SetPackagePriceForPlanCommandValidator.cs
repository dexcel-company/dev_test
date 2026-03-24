using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages.Constants;
using FluentValidation;

namespace CelloPark.Application.Features.Packets.Commands.SetPriceForPlan;

internal sealed class SetPackagePriceForPlanCommandValidator :
    AbstractValidator<SetPackagePriceForPlanCommand>
{
    public SetPackagePriceForPlanCommandValidator()
    {
        // PackageId

        RuleFor(x => x.PackageId)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Invalid, nameof(SetPackagePriceForPlanCommand.PackageId)));

        // PlanId

        RuleFor(x => x.PlanId)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Invalid, nameof(SetPackagePriceForPlanCommand.PlanId)));

        // Price

        RuleFor(x => x.Dto.Price)
            .GreaterThanOrEqualTo(PlanPackageSettings.PriceMinValue)
            .WithMessage(string.Format(string.Format(ErrorDescriptions.TooSmall, nameof(PackagePlanCreateDto.Price))))
            .LessThanOrEqualTo(PlanPackageSettings.PriceMaxValue)
            .WithMessage(string.Format(ErrorDescriptions.TooBig, nameof(PackagePlanCreateDto.Price)));
    }
}
