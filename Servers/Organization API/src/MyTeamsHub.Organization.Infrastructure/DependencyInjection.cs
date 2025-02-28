using System.Reflection;

using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.ConfigureRabbitMQSettings(configuration);
        services.AddRabbitMQ();

        return services;
    }

    public static IServiceCollection ConfigureRabbitMQSettings(this IServiceCollection services, IConfiguration configuration)
    => services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

    private static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<RabbitMQSettings>>().Value;

        services
            .AddScoped<IMessageProducerService, MessageProducerService>()
            .AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                var assembly = Assembly.GetEntryAssembly();
                x.AddConsumers(assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("create-new-team-queue", e =>
                    {
                        e.ConfigureConsumers(context);

                        e.UseRawJsonDeserializer();
                    });
                });
            });

        return services;
    }
}
