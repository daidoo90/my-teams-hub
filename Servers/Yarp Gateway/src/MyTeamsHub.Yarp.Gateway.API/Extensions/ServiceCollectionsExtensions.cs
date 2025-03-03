using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Yarp.ReverseProxy.Health;

namespace MyTeamsHub.Yarp.Gateway.API.Extensions;

internal static class ServiceCollectionsExtensions
{
    internal static IServiceCollection AddDefaultRateLimiterPolicy(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
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

        return services;
    }

    internal static IServiceCollection AddDefaultTimeoutPolicy(this IServiceCollection services)
    {
        services.AddRequestTimeouts(options =>
        {
            options.AddPolicy("default-timeout-policy", TimeSpan.FromSeconds(20));
        });

        return services;
    }

    internal static IServiceCollection AddDefaultHealthCheckOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TransportFailureRateHealthPolicyOptions>(configuration.GetSection("ReverseProxy:HealthCheckOptions"));

        return services;
    }
}
