using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Tokens.Generators.Abstractions;
using CelloPark.Application.Features.Users.Commands.RefreshToken.Abstractions;
using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Users.Commands.RefreshToken;

internal sealed class RefreshUserTokenCommandHandler :
    IRefreshUserTokenCommandHandler
{
    public RefreshUserTokenCommandHandler(
        IManagementContext manageContext,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        TimeProvider timeProvider)
    {
        _managementContext = manageContext;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _timeProvider = timeProvider;
    }

    private readonly IManagementContext _managementContext;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly TimeProvider _timeProvider;

    public async Task<ErrorOr<UserToken>> HandleAsync(
        RefreshUserTokenCommand request, CancellationToken cancellationToken = default)
    {
        User? user = await _managementContext.Users
            .Include(user => user.RefreshSessions)
            .FirstOrDefaultAsync(user => user.RefreshSessions
                .Any(refreshSession => refreshSession.RefreshToken == request.RefreshToken), cancellationToken);

        if (user is null)
        {
            return UserErrors.AccessDenied;
        }

        RefreshSession refreshSession = user.RefreshSessions.First(refreshSession => refreshSession.RefreshToken == request.RefreshToken);
        DateTimeOffset utcNow = _timeProvider.GetUtcNow();

        ErrorOr<None> removeResult = user.RemoveRefreshSession(refreshSession);

        if (removeResult.IsError)
        {
            return removeResult.Errors;
        }

        if (refreshSession.ExpiresIn < utcNow)
        {
            return UserErrors.AccessDenied;
        }

        (string accessToken, long expiresInSeconds) = _accessTokenGenerator.GenerateToken(
            userId: user.Id,
            firstName: user.FirstName,
            lastname: user.LastName,
            utcNow: utcNow);

        string refreshToken = _refreshTokenGenerator.GenerateToken();

        ErrorOr<RefreshSession> refreshSessionResult = user.AddRefreshSession(
            refreshToken: refreshToken,
            userAgent: request.UserAgent,
            fingerprint: request.Fingerprint,
            ipAddress: request.IpAddress,
            createdAt: utcNow.UtcDateTime);

        if (refreshSessionResult.IsError)
        {
            return refreshSessionResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return new UserToken
        {
            AccessToken = accessToken,
            ExpiresIn = expiresInSeconds,
            RefreshToken = refreshSessionResult.Value.RefreshToken,
        };
    }
}
