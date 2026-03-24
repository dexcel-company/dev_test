using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Passwords.Hashers.Abstractions;
using CelloPark.Application.Common.Tokens.Generators.Abstractions;
using CelloPark.Application.Features.Users.Commands.SignUp.Abstractions;
using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Features.Roles;
using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Users.Commands.SignUp;

internal sealed class SignUpUserCommandHandler :
    ISignUpUserCommandHandler
{
    public SignUpUserCommandHandler(
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
        SignUpUserCommand request, CancellationToken cancellationToken = default)
    {
        SignUpUserCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));
        }

        bool exists = await _managementContext.Users
            .AnyAsync(user => EF.Functions.Like(user.Email, $"%{request.Dto.Email}%"), cancellationToken);

        if (exists)
        {
            return UserErrors.EmailAlreadyTaken;
        }

        Role? role = await _managementContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.Name == "Manager", cancellationToken);

        string hashedPassword = _passwordHasher.HashPassword(request.Dto.Password);

        ErrorOr<User> userResult = User.Create(
            firstName: request.Dto.Firstname,
            lastName: request.Dto.Lastname,
            email: request.Dto.Email,
            phoneNumber: null,
            jobTitle: null,
            password: hashedPassword);

        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        userResult.Value.AddRole(role!);

        await _managementContext.Users.AddAsync(userResult.Value, cancellationToken);

        DateTimeOffset utcNow = _timeProvider.GetUtcNow();

        (string accessToken, long expiresIn) = _accessTokenGenerator.GenerateToken(
            userId: userResult.Value.Id,
            firstName: userResult.Value.FirstName,
            lastname: userResult.Value.LastName,
            utcNow: utcNow);

        string refreshToken = _refreshTokenGenerator.GenerateToken();

        ErrorOr<RefreshSession> refreshSessionResult = userResult.Value.AddRefreshSession(
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
