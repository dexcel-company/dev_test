using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.AuditDetails.Update.Errors;

public static class UpdateDetailsErrors
{
    public static Error UpdatedAtIsInvalid => Error.Validation(
        code: $"{nameof(UpdateDetails)}.{nameof(UpdateDetails.UpdatedAt)}",
        description: string.Format(ErrorDescriptions.Invalid, nameof(UpdateDetails.UpdatedAt)));

    public static Error UpdatedByIsInvalid => Error.Validation(
        code: $"{nameof(UpdateDetails)}.{nameof(UpdateDetails.UpdatedBy)}",
        description: string.Format(ErrorDescriptions.Invalid, nameof(UpdateDetails.UpdatedBy)));
}
