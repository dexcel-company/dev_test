using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Plans.Errors;

public static class PlanErrors
{
    public static Error ShadowIdIsTooSmall => Error.Validation(
        code: "Identifier",
        description: "Field 'Identifier' must be greater than or equal to 1.");

    public static Error ShadowIdAlreadyExists => Error.Conflict(
        code: "Identifier",
        description: "Record with the same identifier already exists.");

    public static Error NameIsNull => Error.Validation(
        code: PlanErrorCodes.Name,
        description: string.Format(ErrorDescriptions.Null, nameof(Plan.Name)));

    public static Error NameIsTooShort => Error.Validation(
        code: PlanErrorCodes.Name,
        description: string.Format(ErrorDescriptions.TooShort, nameof(Plan.Name)));

    public static Error NameIsTooLong => Error.Validation(
        code: PlanErrorCodes.Name,
        description: string.Format(ErrorDescriptions.TooLong, nameof(Plan.Name)));

    public static Error DescriptionIsTooLong => Error.Validation(
        code: PlanErrorCodes.Description,
        description: string.Format(ErrorDescriptions.TooLong, nameof(Plan.Description)));

    public static Error PriceIsTooSmall => Error.Validation(
        code: PlanErrorCodes.Price,
        description: string.Format(ErrorDescriptions.TooSmall, nameof(Plan.Price)));

    public static Error PriceIsTooBig => Error.Validation(
        code: PlanErrorCodes.Price,
        description: string.Format(ErrorDescriptions.TooBig, nameof(Plan.Price)));

    public static Error VatIsTooSmall => Error.Validation(
        code: PlanErrorCodes.Vat,
        description: string.Format(ErrorDescriptions.TooSmall, nameof(Plan.Vat)));

    public static Error VatIsTooBig => Error.Validation(
        code: PlanErrorCodes.Vat,
        description: string.Format(ErrorDescriptions.TooBig, nameof(Plan.Vat)));

    public static Error StartDateIsInvalid => Error.Validation(
        code: PlanErrorCodes.StartDate,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Plan.StartDate)));

    public static Error EndDateIsInvalid => Error.Validation(
        code: PlanErrorCodes.EndDate,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Plan.EndDate)));

    public static Error NameAlreadyExists => Error.Conflict(
        code: PlanErrorCodes.Name,
        description: "Plan with the same name already exists.");

    public static Error NotFound => Error.NotFound(
        code: PlanErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(Plan)));
}
