using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Packages.Errors;

public static class PackageErrors
{
    public static Error NameIsNull => Error.Validation(
            code: PackageErrorCodes.Name,
            description: string.Format(ErrorDescriptions.Null, nameof(Package.Name)));

    public static Error NameIsTooShort => Error.Validation(
            code: PackageErrorCodes.Name,
            description: string.Format(ErrorDescriptions.TooShort, nameof(Package.Name)));

    public static Error NameIsTooLong => Error.Validation(
            code: PackageErrorCodes.Name,
            description: string.Format(ErrorDescriptions.TooLong, nameof(Package.Name)));

    public static Error DescriptionIsTooLong => Error.Validation(
            code: PackageErrorCodes.Description,
            description: string.Format(ErrorDescriptions.TooLong, nameof(Package.Description)));

    public static Error StartDateIsInvalid => Error.Validation(
            code: PackageErrorCodes.StartDate,
            description: string.Format(ErrorDescriptions.Invalid, nameof(Package.StartDate)));

    public static Error EndDateIsInvalid => Error.Validation(
            code: PackageErrorCodes.EndDate,
            description: string.Format(ErrorDescriptions.Invalid, nameof(Package.EndDate)));

    public static Error NameAlreadyExists => Error.Conflict(
        code: PackageErrorCodes.Name,
        description: "Package with the same name already exists.");

    public static Error NotFound => Error.NotFound(
            code: PackageErrorCodes.NotFound,
            description: string.Format(ErrorDescriptions.NotFound, nameof(Package)));
}
