using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Roles.Errors;

public static class RoleErrors
{
    public static class Validation
    {
        public static class Name
        {
            public static Error NullOrEmpty => Error.Validation(
                code: RoleErrorCodes.Name,
                description: string.Format(ErrorDescriptions.Null, nameof(Role.Name)));

            public static Error TooShort => Error.Validation(
                code: RoleErrorCodes.Name,
                description: string.Format(ErrorDescriptions.TooShort, nameof(Role.Name)));

            public static Error TooLong => Error.Validation(
                code: RoleErrorCodes.Name,
                description: string.Format(ErrorDescriptions.TooLong, nameof(Role.Name)));
        }
    }
}
