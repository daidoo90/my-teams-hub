using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyTeamsHub.Core.Application.Interfaces.Shared;
using MyTeamsHub.Infrastructure.Cache;
using MyTeamsHub.Infrastructure.Messaging;
using MyTeamsHub.Infrastructure.Services.Shared;

namespace MyTeamsHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICryptoService, CryptoService>();

        //var cacheConnectionString = configuration.GetRequiredSection(nameof(ConnectionStrings)).GetValue<string>(nameof(ConnectionStrings.Cache))!;

        services.AddCache();
        services.AddRabbitMQ();

        return services;
    }
}
