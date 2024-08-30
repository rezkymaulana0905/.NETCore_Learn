using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PTC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace PTC.Utils;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
}

public class JWT
{
    public static string GenerateJwtToken(IOptions<JwtSettings> jwtSettings, User foundUser, string address, UserRecord? record = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Value.SecretKey);
        var claims = new[]
        {
        new Claim("UserName", foundUser.Username),
        new Claim("UserId", foundUser.Id.ToString()),
        new Claim("LoginId", record != null ? record.Id.ToString() : "No Login ID"),
        new Claim("Address", address)
    };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
