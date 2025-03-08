using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MyTeamsHub.Yarp.Gateway.API.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOutputCache(options =>
    {
        options.AddPolicy("default-cache-policy", builder => builder.Expire(TimeSpan.FromSeconds(20)));
    })
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddDefaultRateLimiterPolicy()
    .AddDefaultTimeoutPolicy()
    .AddDefaultHealthCheckOptions(builder.Configuration)
    .AddHealthChecks();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "My Teams Hub Issuer",// Configuration["Jwt:Issuer"],
            ValidAudience = "My Teams Hub Audience", //Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("at Microsoft.IdentityModel.Tokens.SymmetricSignatureProvider..ctor(SecurityKey key, String algorithm, Boolean willCreateSignatures)"))//Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedOnly", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

    options.AddPolicy("Nomenclatures", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());
});

var app = builder.Build();

app.MapGet("login", () =>
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("at Microsoft.IdentityModel.Tokens.SymmetricSignatureProvider..ctor(SecurityKey key, String algorithm, Boolean willCreateSignatures)"));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: "My Teams Hub Issuer",
        audience: "My Teams Hub Audience",
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new { token = tokenString });
});

app
    .UseRateLimiter()
    .UseRequestTimeouts()
    .UseOutputCache();

app.MapHealthChecks("/proxy-health");

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();
