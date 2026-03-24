using CelloPark.Application.Features.Users.Dtos;
using System.Security.Claims;

namespace CelloPark.Api.Common.Endpoints.Extensions;

public static class HttpContextExtensions
{
    public static UserInformationDto GetUserInformation(this HttpContext httpContext)
    {
        return new UserInformationDto
        {
            UserId = GetUserId(httpContext),
            UserAgent = GetUserAgent(),
            Fingerprint = GetFingerprint(),
            IpAddress = GetIpAddress()
        };
    }

    private static Guid? GetUserId(HttpContext httpContext)
    {
        Claim? claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is null)
        {
            return null;
        }

        bool isParseed = Guid.TryParse(claim.Value, out Guid userId);

        if (!isParseed)
        {
            return null;
        }

        return userId;
    }

    private static string? GetUserAgent()
    {
        return "UserAgent";
    }

    private static string? GetFingerprint()
    {
        return "Fingerprint";
    }

    private static string? GetIpAddress()
    {
        return "IpAddress";
    }
}
