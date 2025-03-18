using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MyTeamsHub.Yarp.Gateway.API.Extensions;

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
            ValidIssuer = "http://identity-server-keycloak:8080/realms/my-teams-hub",// Configuration["Jwt:Issuer"],
            ValidAudience = "account", //Configuration["Jwt:Audience"],
        };

        options.Authority = "http://identity-server-keycloak:8080/realms/my-teams-hub";
        options.RequireHttpsMetadata = false;
    });

builder
    .Services
    .AddAuthorizationBuilder()
    .AddPolicy("AuthenticatedOnly", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

var app = builder.Build();

app
    .UseRateLimiter()
    .UseRequestTimeouts()
    .UseOutputCache();

app.MapHealthChecks("/proxy-health");

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();
