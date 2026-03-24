using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Passwords.Hashers.Abstractions;
using CelloPark.Application.Common.Tokens.Generators.Abstractions;
using CelloPark.Application.Features.Users.Commands.SignIn.Abstractions;
using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Users.Commands.SignIn;

internal sealed class SignInUserCommandHandler :
    ISignInUserCommandHandler
{
    public SignInUserCommandHandler(
        IManagementContext manageContext,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IPasswordHasher passwordHasher,
        TimeProvider timeProvider)
    {
        _managementContext = manageContext;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _passwordHasher = passwordHasher;
        _timeProvider = timeProvider;
    }

    private readonly IManagementContext _managementContext;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly TimeProvider _timeProvider;

    public async Task<ErrorOr<UserToken>> HandleAsync(
        SignInUserCommand request, CancellationToken cancellationToken = default)
    {
        SignInUserCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));
        }

        User? user = await _managementContext.Users
            .FirstOrDefaultAsync(user => EF.Functions.Like(user.Email, $"%{request.Dto.Email}%"), cancellationToken);

        if (user is null)
        {
            return UserErrors.Unauthorized;
        }

        bool isVerified = _passwordHasher.VerifyHashedPassword(request.Dto.Password, user.Password);

        if (!isVerified)
        {
            return UserErrors.Unauthorized;
        }

        DateTimeOffset utcNow = _timeProvider.GetUtcNow();

        (string accessToken, long expiresIn) = _accessTokenGenerator.GenerateToken(
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
            ExpiresIn = expiresIn,
            RefreshToken = refreshSessionResult.Value.RefreshToken,
        };
    }
}
