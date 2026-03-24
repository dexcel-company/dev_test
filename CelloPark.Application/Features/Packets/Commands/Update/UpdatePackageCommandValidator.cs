using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Constants;
using FluentValidation;

namespace CelloPark.Application.Features.Packets.Commands.Update;

internal sealed class UpdatePackageCommandValidator :
    AbstractValidator<UpdatePackageCommand>
{
    public UpdatePackageCommandValidator()
    {
        // ShadowId

        RuleFor(x => x.Dto.ShadowId)
            .GreaterThanOrEqualTo(PackageSettings.ShadowIdMinValue)
            .When(x => x.Dto.ShadowId is not null)
            .WithMessage("Field 'Identifier' must be greater than or equal to 1.");

        // Name

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Package.Name)))
            .MinimumLength(PackageSettings.NameMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(Package.Name)))
            .MaximumLength(PackageSettings.NameMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(Package.Name)));

        // Description

        RuleFor(x => x.Dto.Description)
            .MaximumLength(PackageSettings.DescriptionMaxLength)
            .When(x => x.Dto.Description is not null)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(Package.Description)));

        // StartDate

        RuleFor(x => x.Dto.StartDate)
            .NotEmpty()
            .When(x => x.Dto.StartDate is not null)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Package.StartDate)))
            .Must(x => x != DateOnly.MinValue && x != DateOnly.MaxValue)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Package.StartDate)));

        // EndStart

        RuleFor(x => x.Dto.EndDate)
            .NotEmpty()
            .When(x => x.Dto.EndDate is not null)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Package.EndDate)))
            .Must(x => x != DateOnly.MinValue && x != DateOnly.MaxValue)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Package.EndDate)));
    }
}
