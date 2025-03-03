using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.AddFixedWindowLimiter("default-rate-limiter-policy", opt =>
    {
        opt.PermitLimit = 4;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

builder.Services.AddRequestTimeouts(options =>
{
    options.AddPolicy("default-timeout-policy", TimeSpan.FromSeconds(20));
});

var app = builder.Build();

app.UseRateLimiter();

app.UseRequestTimeouts();

app.MapReverseProxy();

app.Run();
