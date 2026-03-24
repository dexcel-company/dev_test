using CelloPark.Domain.Features.Users.Entities.RefreshSessions.Constants;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions.Errors;
using CelloPark.Domain.Features.Users.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Users.Entities.RefreshSessions;

public sealed class RefreshSession
{
    private RefreshSession() { }

    private RefreshSession(
        Guid userId,
        string refreshToken,
        string userAgent,
        string fingerprint,
        string ipAddress,
        DateTime createdAt)
    {
        UserId = userId;
        RefreshToken = refreshToken;
        UserAgent = userAgent;
        Fingerprint = fingerprint;
        IpAddress = ipAddress;
        CreatedAt = createdAt;
        ExpiresIn = CreatedAt.AddDays(RefreshSessionSettings.ExpiresInDays);
    }

    public Guid Id { get; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; } = null!;
    public string UserAgent { get; private set; } = null!;
    public string Fingerprint { get; private set; } = null!;
    public string IpAddress { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresIn { get; private set; }

    public static ErrorOr<RefreshSession> Create(
        Guid userId,
        string refreshToken,
        string userAgent,
        string fingerprint,
        string ipAddress,
        DateTime createdAt)
    {
        List<Error> errors = [];

        ErrorOr<Guid> userIdResult = ValidateUserId(userId);

        if (userIdResult.IsError)
        {
            errors.Add(userIdResult.FirstError);
        }

        ErrorOr<string> refreshTokenResult = ValidateRefreshToken(refreshToken);

        if (refreshTokenResult.IsError)
        {
            errors.Add(refreshTokenResult.FirstError);
        }

        ErrorOr<string> userAgentResult = ValidateUserAgent(userAgent);

        if (userAgentResult.IsError)
        {
            errors.Add(userAgentResult.FirstError);
        }

        ErrorOr<string> fingerprintResult = ValidateFingerprint(fingerprint);

        if (fingerprintResult.IsError)
        {
            errors.Add(fingerprintResult.FirstError);
        }

        ErrorOr<string> ipAddressResult = ValidateIpAddress(ipAddress);

        if (ipAddressResult.IsError)
        {
            errors.Add(ipAddressResult.FirstError);
        }

        ErrorOr<DateTime> createdAtResult = ValidateCreatedAt(createdAt);

        if (createdAtResult.IsError)
        {
            errors.Add(createdAtResult.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new RefreshSession(
            userId: userIdResult.Value,
            refreshToken: refreshTokenResult.Value,
            userAgent: userAgentResult.Value,
            fingerprint: fingerprintResult.Value,
            ipAddress: ipAddressResult.Value,
            createdAt: createdAtResult.Value);
    }

    private static ErrorOr<Guid> ValidateUserId(Guid userId)
    {
        if (userId == default)
        {
            return UserErrors.IdIsInvalid;
        }

        return userId;
    }

    private static ErrorOr<string> ValidateRefreshToken(string refreshToken)
    {
        if (refreshToken is null)
        {
            return RefreshSessionErrors.RefreshTokenIsNull;
        }

        if (refreshToken.Length < RefreshSessionSettings.RefreshTokenMinLength)
        {
            return RefreshSessionErrors.RefreshTokenIsTooShort;
        }

        if (refreshToken.Length > RefreshSessionSettings.RefreshTokenMaxLength)
        {
            return RefreshSessionErrors.RefreshTokenIsTooLong;
        }

        return refreshToken;
    }

    private static ErrorOr<string> ValidateUserAgent(string userAgent)
    {
        if (userAgent is null)
        {
            return RefreshSessionErrors.UserAgentIsNull;
        }

        if (userAgent.Length < RefreshSessionSettings.UserAgentMinLength)
        {
            return RefreshSessionErrors.UserAgentIsTooShort;
        }

        if (userAgent.Length > RefreshSessionSettings.UserAgentMaxLength)
        {
            return RefreshSessionErrors.UserAgentIsTooLong;
        }

        return userAgent;
    }

    private static ErrorOr<string> ValidateFingerprint(string fingerprint)
    {
        if (fingerprint is null)
        {
            return RefreshSessionErrors.FingerprintIsTooLong;
        }

        if (fingerprint.Length < RefreshSessionSettings.FingerPrintMinLength)
        {
            return RefreshSessionErrors.FingerprintIsTooLong;
        }

        if (fingerprint.Length > RefreshSessionSettings.FingerPrintMaxLength)
        {
            return RefreshSessionErrors.FingerprintIsTooLong;
        }

        return fingerprint;
    }

    private static ErrorOr<string> ValidateIpAddress(string ipAddress)
    {
        if (ipAddress is null)
        {
            return RefreshSessionErrors.IpAdressIsNull;
        }

        if (ipAddress.Length < RefreshSessionSettings.IpAddressMinLength)
        {
            return RefreshSessionErrors.IpAdressIsTooShort;
        }

        if (ipAddress.Length > RefreshSessionSettings.IpAddressMaxLength)
        {
            return RefreshSessionErrors.IpAdressIsTooLong;
        }

        return ipAddress;
    }

    private static ErrorOr<DateTime> ValidateCreatedAt(DateTime createdAt)
    {
        if (createdAt == default || createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue || createdAt > DateTime.UtcNow)
        {
            return RefreshSessionErrors.CreatedAtIsInvalid;
        }

        return createdAt;
    }
}
