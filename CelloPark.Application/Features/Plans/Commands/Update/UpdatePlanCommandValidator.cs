using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Plans.Constants;
using FluentValidation;

namespace CelloPark.Application.Features.Plans.Commands.Update;

internal sealed class UpdatePlanCommandValidator :
    AbstractValidator<UpdatePlanCommand>
{
    public UpdatePlanCommandValidator()
    {
        // ShadowId

        RuleFor(x => x.Dto.ShadowId)
            .GreaterThanOrEqualTo(PlanSettings.ShadowIdMinValue)
            .When(x => x.Dto.ShadowId is not null)
            .WithMessage("Field 'Identifier' must be greater than or equal to 1.");

        // Name

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Plan.Name)))
            .MinimumLength(PlanSettings.NameMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(Plan.Name)))
            .MaximumLength(PlanSettings.NameMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(Plan.Name)));

        // Description

        RuleFor(x => x.Dto.Description)
            .MaximumLength(PlanSettings.DescriptionMaxLength)
            .When(x => x.Dto.Description is not null)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(Plan.Description)));

        // Price

        RuleFor(x => x.Dto.Price)
            .GreaterThanOrEqualTo(PlanSettings.PriceMinValue)
            .WithMessage(string.Format(ErrorDescriptions.TooSmall, nameof(Plan.Price)))
            .LessThanOrEqualTo(PlanSettings.PriceMaxValue)
            .WithMessage(string.Format(ErrorDescriptions.TooBig, nameof(Plan.Price)));

        // StartDate

        RuleFor(x => x.Dto.StartDate)
            .NotEmpty()
            .When(x => x.Dto.StartDate is not null)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Plan.StartDate)))
            .Must(x => x != DateOnly.MinValue && x != DateOnly.MaxValue)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Plan.StartDate)));

        // EndStart

        RuleFor(x => x.Dto.EndDate)
            .NotEmpty()
            .When(x => x.Dto.EndDate is not null)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Plan.EndDate)))
            .Must(x => x != DateOnly.MinValue && x != DateOnly.MaxValue)
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Plan.EndDate)));
    }
}
