using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clini_Management_System.Server.Helpers;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) Generate(User user);
}

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    #region Fields

    private readonly JwtSettings _settings;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    #endregion

    #region Constructor

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    #endregion

    #region Public Methods

    public (string Token, DateTime ExpiresAt) Generate(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiresMinutes);
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: BuildClaims(user),
            expires: expiresAt,
            signingCredentials: _credentials);

        return (_tokenHandler.WriteToken(token), expiresAt);
    }

    #endregion

    #region Private Methods

    private static IEnumerable<Claim> BuildClaims(User user) =>
    [
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.UniqueName, user.Username),
        new(TenantContext.ClinicIdClaim, user.ClinicId.ToString()),
        new(ClaimTypes.Role, user.Role)
    ];

    #endregion
}
