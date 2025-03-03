using MyTeamsHub.Yarp.Gateway.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddDefaultRateLimiterPolicy()
    .AddDefaultTimeoutPolicy()
    .AddDefaultHealthCheckOptions(builder.Configuration);

var app = builder.Build();

app.UseRateLimiter();

app.UseRequestTimeouts();

app.MapReverseProxy();

app.Run();
