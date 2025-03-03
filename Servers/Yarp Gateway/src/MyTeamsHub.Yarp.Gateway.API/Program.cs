using MyTeamsHub.Yarp.Gateway.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddDefaultRateLimiterPolicy()
    .AddDefaultTimeoutPolicy()
    .AddDefaultHealthCheckOptions(builder.Configuration)
    .AddHealthChecks();

var app = builder.Build();

app
    .UseRateLimiter()
    .UseRequestTimeouts();

app.MapHealthChecks("/proxy-health");

app.MapReverseProxy();

app.Run();
