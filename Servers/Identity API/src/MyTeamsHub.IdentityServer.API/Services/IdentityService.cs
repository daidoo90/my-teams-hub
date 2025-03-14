using Microsoft.IdentityModel.Tokens;
using MyTeamsHub.IdentityServer.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTeamsHub.IdentityServer.API.Services;

public class IdentityService : IIdentityService
{
    public UserToken GetToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes("at Microsoft.IdentityModel.Tokens.SymmetricSignatureProvider..ctor(SecurityKey key, String algorithm, Boolean willCreateSignatures)");// RandomNumberGenerator.GetBytes(128);

        var expires = DateTime.UtcNow.AddHours(1);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub,"test12"),// _appSettings.AuthenticationConfiguration.Jwt.Secret);,
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("userId", user.UserId.ToString())
        }),

            Expires = expires,

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "My Teams Hub Issuer",//_appSettings.AuthenticationConfiguration.Jwt.Issuer,
            Audience = "My Teams Hub Audience",//_appSettings.AuthenticationConfiguration.Jwt.Audience
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var token = tokenHandler.WriteToken(securityToken);

        return new UserToken
        {
            AccessToken = token
        };
    }
}

public interface IIdentityService
{
    UserToken GetToken(User user);
}