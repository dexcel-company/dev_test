using CelloPark.Application.Features.Items.Dtos;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Items;
using CelloPark.Domain.Features.Items.Constants;
using FluentValidation;

namespace CelloPark.Api.Features.Items.Validators;

public sealed class CreateItemValidator :
    AbstractValidator<ItemCreateDto>
{
    public CreateItemValidator()
    {
        // ShadowId

        RuleFor(x => x.ShadowId)
            .GreaterThanOrEqualTo(1)
            .When(x => x.ShadowId is not null)
            .WithMessage("Field 'Identifier' must be greater than or equal to 1.");

        // Name

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(Item.Name)))
            .MinimumLength(ItemSettings.NameMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(Item.Name)))
            .MaximumLength(ItemSettings.NameMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(Item.Name)));

        // Description

        RuleFor(x => x.Description)
            .MaximumLength(ItemSettings.DescriptionMaxLength)
            .When(x => x.Description is not null)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(Item.Description)));
    }
}
