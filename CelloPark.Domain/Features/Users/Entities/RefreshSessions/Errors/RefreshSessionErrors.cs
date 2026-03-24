using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Users.Entities.RefreshSessions.Errors;

public static class RefreshSessionErrors
{
    public static Error RefreshTokenIsNull => Error.Validation(
        code: RefreshSessionErrorCodes.RefreshToken,
        description: string.Format(ErrorDescriptions.Null, nameof(RefreshSession.RefreshToken)));

    public static Error RefreshTokenIsTooShort => Error.Validation(
        code: RefreshSessionErrorCodes.RefreshToken,
        description: string.Format(ErrorDescriptions.TooShort, nameof(RefreshSession.RefreshToken)));

    public static Error RefreshTokenIsTooLong => Error.Validation(
        code: RefreshSessionErrorCodes.RefreshToken,
        description: string.Format(ErrorDescriptions.TooLong, nameof(RefreshSession.RefreshToken)));

    public static Error UserAgentIsNull => Error.Validation(
        code: RefreshSessionErrorCodes.UserAgent,
        description: string.Format(ErrorDescriptions.Null, nameof(RefreshSession.UserAgent)));

    public static Error UserAgentIsTooShort => Error.Validation(
        code: RefreshSessionErrorCodes.UserAgent,
        description: string.Format(ErrorDescriptions.TooShort, nameof(RefreshSession.UserAgent)));

    public static Error UserAgentIsTooLong => Error.Validation(
        code: RefreshSessionErrorCodes.UserAgent,
        description: string.Format(ErrorDescriptions.TooLong, nameof(RefreshSession.UserAgent)));

    public static Error FingerprintIsNull => Error.Validation(
        code: RefreshSessionErrorCodes.Fingerprint,
        description: string.Format(ErrorDescriptions.Null, nameof(RefreshSession.Fingerprint)));

    public static Error FingerprintIsTooShort => Error.Validation(
        code: RefreshSessionErrorCodes.Fingerprint,
        description: string.Format(ErrorDescriptions.TooShort, nameof(RefreshSession.Fingerprint)));

    public static Error FingerprintIsTooLong => Error.Validation(
        code: RefreshSessionErrorCodes.Fingerprint,
        description: string.Format(ErrorDescriptions.TooLong, nameof(RefreshSession.Fingerprint)));

    public static Error IpAdressIsNull => Error.Validation(
        code: RefreshSessionErrorCodes.IpAddress,
        description: string.Format(ErrorDescriptions.Null, nameof(RefreshSession.IpAddress)));

    public static Error IpAdressIsTooShort => Error.Validation(
        code: RefreshSessionErrorCodes.IpAddress,
        description: string.Format(ErrorDescriptions.TooShort, nameof(RefreshSession.IpAddress)));

    public static Error IpAdressIsTooLong => Error.Validation(
        code: RefreshSessionErrorCodes.IpAddress,
        description: string.Format(ErrorDescriptions.TooLong, nameof(RefreshSession.IpAddress)));

    public static Error CreatedAtIsInvalid => Error.Validation(
        code: RefreshSessionErrorCodes.CreatedAt,
        description: string.Format(ErrorDescriptions.Invalid, nameof(RefreshSession.CreatedAt)));

    public static Error NotFound => Error.NotFound(
        code: RefreshSessionErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(RefreshSession)));
}
