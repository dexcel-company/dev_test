using CelloPark.Application.Common.Tokens.Generators.Abstractions;
using CelloPark.Infrastructure.Common.Tokens.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CelloPark.Infrastructure.Common.Tokens.Generators;

internal sealed class AccessTokenGenerator : IAccessTokenGenerator
{
    public AccessTokenGenerator(IOptions<AccessTokenOptions> accessTokenOptions)
    {
        _accessTokenOptions = accessTokenOptions.Value;
    }

    private readonly AccessTokenOptions _accessTokenOptions;

    public (string, long) GenerateToken(Guid userId, string? firstName, string? lastName, DateTimeOffset utcNow)
    {
        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_accessTokenOptions.Secret));
        SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId.ToString(), ClaimValueTypes.String),
            new(JwtRegisteredClaimNames.Name, $"{firstName} {lastName}", ClaimValueTypes.String),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
            new(JwtRegisteredClaimNames.Iat, utcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
        ];

        JwtSecurityToken securityToken = new(
            expires: utcNow.UtcDateTime.AddSeconds(_accessTokenOptions.ExpiresIn),
            claims: claims,
            signingCredentials: signingCredentials);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return (token, _accessTokenOptions.ExpiresIn);
    }
}
