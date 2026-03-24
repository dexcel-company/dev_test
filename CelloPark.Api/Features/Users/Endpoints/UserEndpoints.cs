using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Api.Common.Endpoints;
using CelloPark.Api.Common.Endpoints.Extensions;
using CelloPark.Application.Features.Users.Commands.RefreshToken;
using CelloPark.Application.Features.Users.Commands.RefreshToken.Abstractions;
using CelloPark.Application.Features.Users.Commands.SignIn;
using CelloPark.Application.Features.Users.Commands.SignIn.Abstractions;
using CelloPark.Application.Features.Users.Commands.SignOut;
using CelloPark.Application.Features.Users.Commands.SignOut.Abstractions;
using CelloPark.Application.Features.Users.Commands.SignUp;
using CelloPark.Application.Features.Users.Commands.SignUp.Abstractions;
using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions.Constants;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Users.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Users");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/users")
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapPost("/sign-up", SignUp)
            .Produces<UserToken>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status409Conflict)
            .WithName(nameof(SignUp))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Sign up.",
            });

        groupV1
            .MapPost("/sign-in", SignIn)
            .Produces<UserToken>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(SignIn))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Sign in.",
            });

        groupV1
            .MapPost("/sign-out", SignOut)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)
            .RequireAuthorization()
            .WithName(nameof(SignOut))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Sign out.",
            });

        groupV1
            .MapPost("/refresh-token", RefreshToken)
            .Produces<UserToken>(StatusCodes.Status200OK)
            .Produces<ProblemHttpResult>(StatusCodes.Status403Forbidden)
            .WithName(nameof(RefreshToken))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Refresh access token.",
            });
    }

    private static async Task<IResult> SignUp(
        [FromBody]
        UserSignUpDto dto,
        [FromServices] ISignUpUserCommandHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        UserInformationDto userInformation = httpContext.GetUserInformation();

        if (userInformation.UserAgent is null
            || userInformation.Fingerprint is null
            || userInformation.IpAddress is null)
        {
            return Results.Forbid();
        }

        SignUpUserCommand request = new(
            dto: dto,
            userAgent: userInformation.UserAgent,
            fingerprint: userInformation.Fingerprint,
            ipAddress: userInformation.IpAddress);

        ErrorOr<UserToken> result = await handler.HandleAsync(request, cancellationToken);

        if (!result.IsError)
        {
            AppendRefreshTokenToCookies(httpContext, result.Value.RefreshToken);
        }

        return result.Match(
            value => Results.CreatedAtRoute(routeName: nameof(SignIn), value: value),
            errors => ErrorResults.Problem(errors));
    }

    private static async Task<IResult> SignIn(
        [FromBody] UserSignInDto dto,
        [FromServices] ISignInUserCommandHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        UserInformationDto userInformation = httpContext.GetUserInformation();

        if (userInformation.UserAgent is null
            || userInformation.Fingerprint is null
            || userInformation.IpAddress is null)
        {
            return Results.Unauthorized();
        }

        SignInUserCommand request = new(
            dto: dto,
            userAgent: userInformation.UserAgent,
            fingerprint: userInformation.Fingerprint,
            ipAddress: userInformation.IpAddress);

        ErrorOr<UserToken> result = await handler.HandleAsync(request, cancellationToken);

        if (!result.IsError)
        {
            AppendRefreshTokenToCookies(httpContext, result.Value.RefreshToken);
        }

        return result.Match(
            value => Results.Ok(value),
            errors => ErrorResults.Problem(errors));
    }

    private static async Task<IResult> SignOut(
        [FromServices] ISignOutUserCommandHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        string? refreshToken = httpContext.Request.Cookies[nameof(RefreshSession.RefreshToken)];
        UserInformationDto userInformation = httpContext.GetUserInformation();

        if (refreshToken is null || userInformation.UserId is null)
        {
            return Results.Unauthorized();
        }

        SignOutUserCommand request = new(userInformation.UserId.Value, refreshToken);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        if (!result.IsError)
        {
            RemoveRefreshTokenFromCookies(httpContext, nameof(RefreshSession.RefreshToken));
        }

        return result.Match(
            _ => Results.NoContent(),
            ErrorResults.Problem);
    }

    private static async Task<IResult> RefreshToken(
        [FromServices] IRefreshUserTokenCommandHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        string? refreshToken = httpContext.Request.Cookies[nameof(RefreshSession.RefreshToken)];
        UserInformationDto userInformation = httpContext.GetUserInformation();

        if (refreshToken is null
            || userInformation.UserAgent is null
            || userInformation.Fingerprint is null
            || userInformation.IpAddress is null)
        {
            return Results.Forbid();
        }

        RefreshUserTokenCommand request = new(
            refreshToken: refreshToken,
            userAgent: userInformation.UserAgent,
            fingerprint: userInformation.Fingerprint,
            ipAddress: userInformation.IpAddress);

        ErrorOr<UserToken> result = await handler.HandleAsync(request, cancellationToken);

        if (!result.IsError)
        {
            AppendRefreshTokenToCookies(httpContext, result.Value.RefreshToken);
        }

        return result.Match(
            value => Results.Ok(value),
            errors => ErrorResults.Problem(errors));
    }

    private static void AppendRefreshTokenToCookies(HttpContext httpContext, string value)
    {
        CookieOptions cookieOptions = CreateCookieOptions();

        httpContext.Response.Cookies.Append(nameof(RefreshSession.RefreshToken), value, cookieOptions);
    }

    private static void RemoveRefreshTokenFromCookies(HttpContext httpContext, string key)
    {
        CookieOptions cookieOptions = CreateCookieOptions();

        httpContext.Response.Cookies.Delete(key, cookieOptions);
    }

    private static CookieOptions CreateCookieOptions()
    {
        return new CookieOptions
        {
            SameSite = SameSiteMode.None,
            MaxAge = TimeSpan.FromDays(RefreshSessionSettings.ExpiresInDays),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            Path = "/api/v1/users",
        };
    }
}
