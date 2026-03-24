using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete.Errors;

public static class DeleteDetailsErrors
{
    public static Error DeletedAtIsInvalid => Error.Validation(
        code: $"{nameof(DeleteDetails)}.{nameof(DeleteDetails.DeletedAt)}",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DeleteDetails.DeletedAt)));

    public static Error DeletedByIsInvalid => Error.Validation(
        code: $"{nameof(DeleteDetails)}.{nameof(DeleteDetails.DeletedBy)}",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DeleteDetails.DeletedBy)));
}
