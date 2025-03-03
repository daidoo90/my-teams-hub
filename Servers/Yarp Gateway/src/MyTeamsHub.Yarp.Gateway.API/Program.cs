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

var app = builder.Build();

app
    .UseRateLimiter()
    .UseRequestTimeouts()
    .UseOutputCache();

app.MapHealthChecks("/proxy-health");

app.MapReverseProxy();

app.Run();
