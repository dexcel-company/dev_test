using CelloPark.Application.Features.Users.ActionContexts.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CelloPark.Infrastructure.Common.ActionContexts;

internal sealed class UserActionContext : IUserActionContext
{
    public UserActionContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? AccessToken
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }

            return _httpContextAccessor
                .HttpContext
                .Request
                .Headers
                .Authorization
                .ToString()
                .Replace("Bearer ", string.Empty);
        }
    }

    public Guid? UserId
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }

            Claim? claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is null)
            {
                return null;
            }

            bool isParsed = Guid.TryParse(claim.Value, out Guid userId);

            if (!isParsed)
            {
                return null;
            }

            return userId;
        }
    }

    public string? UserAgent => "UserAgent";
    public string? Fingerprint => "Fingerprint";
    public string? IpAddress => "IpAddress";

    private readonly IHttpContextAccessor _httpContextAccessor;
}