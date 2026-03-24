using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.AuditDetails.Create.Errors;

public static class CreationDetailErrors
{
    public static Error CreatedAtIsInvalid => Error.Validation(
        code: $"{nameof(CreateDetails)}.{nameof(CreateDetails.CreatedAt)}",
        description: string.Format(ErrorDescriptions.Invalid, nameof(CreateDetails.CreatedAt)));

    public static Error CreatedByIsInvalid => Error.Validation(
        code: $"{nameof(CreateDetails)}.{nameof(CreateDetails.CreatedBy)}",
        description: string.Format(ErrorDescriptions.Invalid, nameof(CreateDetails.CreatedBy)));
}
