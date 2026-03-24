using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Items.Errors;

public static class ItemErrors
{
    public static Error NameIsNull => Error.Validation(
        code: ItemErrorCodes.Name,
        description: string.Format(ErrorDescriptions.Null, nameof(Item.Name)));

    public static Error NameIsTooShort => Error.Validation(
        code: ItemErrorCodes.Name,
        description: string.Format(ErrorDescriptions.TooShort, nameof(Item.Name)));

    public static Error NameIsTooLong => Error.Validation(
        code: ItemErrorCodes.Name,
        description: string.Format(ErrorDescriptions.TooLong, nameof(Item.Name)));

    public static Error DescriptionIsTooLong => Error.Validation(
        code: ItemErrorCodes.Description,
        description: string.Format(ErrorDescriptions.TooLong, nameof(Item.Description)));

    public static Error NotFound => Error.NotFound(
            code: ItemErrorCodes.NotFound,
            description: string.Format(ErrorDescriptions.NotFound, nameof(Item)));
}
