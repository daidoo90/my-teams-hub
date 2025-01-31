using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyTeamsHub.APIs.Core.Configurations;

public static class CorsConfiguration
{
    public const string CorsPolicy = "CostPolicy";

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy,
                builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Patch, HttpMethods.Options));
        });
    }
}
