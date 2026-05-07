using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clini_Management_System.Server.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Clini_Management_System.Server.Helpers;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) Generate(User user);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAt) Generate(User user)
    {
        var section = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]!));
        var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(section["ExpiresMinutes"]!));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new("clinicId", user.ClinicId.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: section["Issuer"],
            audience: section["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
