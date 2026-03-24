using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Users.Errors;

public static class UserErrors
{
    public static Error IdIsInvalid => Error.Validation(
        code: UserErrorCodes.Id,
        description: string.Format(ErrorDescriptions.Null, nameof(User.Id)));

    public static Error FirstNameIsNull => Error.Validation(
        code: UserErrorCodes.FirstName,
        description: string.Format(ErrorDescriptions.Null, nameof(User.FirstName)));

    public static Error FirstNameIsTooShort => Error.Validation(
        code: UserErrorCodes.FirstName,
        description: string.Format(ErrorDescriptions.TooShort, nameof(User.FirstName)));

    public static Error FirstNameIsTooLong => Error.Validation(
        code: UserErrorCodes.FirstName,
        description: string.Format(ErrorDescriptions.TooLong, nameof(User.FirstName)));

    public static Error LastNameIsNull => Error.Validation(
        code: UserErrorCodes.LastName,
        description: string.Format(ErrorDescriptions.Null, nameof(User.LastName)));

    public static Error LastNameIsTooShort => Error.Validation(
        code: UserErrorCodes.LastName,
        description: string.Format(ErrorDescriptions.TooShort, nameof(User.LastName)));

    public static Error LastNameIsTooLong => Error.Validation(
        code: UserErrorCodes.LastName,
        description: string.Format(ErrorDescriptions.TooLong, nameof(User.LastName)));

    public static Error EmailIsNull => Error.Validation(
        code: UserErrorCodes.Email,
        description: string.Format(ErrorDescriptions.Null, nameof(User.Email)));

    public static Error EmailIsTooShort => Error.Validation(
        code: UserErrorCodes.Email,
        description: string.Format(ErrorDescriptions.TooShort, nameof(User.Email)));

    public static Error EmailIsTooLong => Error.Validation(
        code: UserErrorCodes.Email,
        description: string.Format(ErrorDescriptions.TooLong, nameof(User.Email)));

    public static Error EmailIsInvalid => Error.Validation(
        code: UserErrorCodes.Email,
        description: string.Format(ErrorDescriptions.Invalid, nameof(User.Email)));

    public static Error PhoneNumberIsTooShort => Error.Validation(
        code: UserErrorCodes.PhoneNumber,
        description: string.Format(ErrorDescriptions.TooShort, nameof(User.PhoneNumber)));

    public static Error PhoneNumberIsTooLong => Error.Validation(
        code: UserErrorCodes.PhoneNumber,
        description: string.Format(ErrorDescriptions.TooLong, nameof(User.PhoneNumber)));

    public static Error PhoneNumberIsInvalid => Error.Validation(
        code: UserErrorCodes.PhoneNumber,
        description: string.Format(ErrorDescriptions.Invalid, nameof(User.PhoneNumber)));

    public static Error JobTitleIsTooShort => Error.Validation(
        code: UserErrorCodes.JobTitle,
        description: string.Format(ErrorDescriptions.TooShort, nameof(User.JobTitle)));

    public static Error JobTitleIsTooLong => Error.Validation(
        code: UserErrorCodes.JobTitle,
        description: string.Format(ErrorDescriptions.TooLong, nameof(User.JobTitle)));

    public static Error PasswordIsNull => Error.Validation(
        code: UserErrorCodes.Password,
        description: string.Format(ErrorDescriptions.Null, nameof(User.Password)));

    public static Error PasswordIsTooShort => Error.Validation(
        code: UserErrorCodes.Password,
        description: string.Format(ErrorDescriptions.TooShort, nameof(User.Password)));

    public static Error PasswordIsTooLong => Error.Validation(
        code: UserErrorCodes.Password,
        description: string.Format(ErrorDescriptions.TooLong, nameof(User.Password)));

    public static Error Unauthorized =>
        Error.Unauthorized(
            code: string.Empty,
            description: string.Empty);

    public static Error NotFound => Error.NotFound(
        code: UserErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(User)));

    public static Error EmailAlreadyTaken => Error.Conflict(
        code: UserErrorCodes.Email,
        description: UserErrorDescriptions.EmailAlreadyTaken);

    public static Error AccessDenied => Error.Forbidden(
        code: string.Empty,
        description: string.Empty);
}
