using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MyTeamsHub.Core.Application.Interfaces;
using MyTeamsHub.Core.Application.Team.Create;

namespace MyTeamsHub.Infrastructure.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureRabbitMQSettings(this IServiceCollection services, IConfiguration configuration)
    => services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

    internal static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<RabbitMQSettings>>().Value;

        services
            .AddScoped<IMessageProducerService, MessageProducerService>()
            .AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.Host(settings.Host, settings.VirtualHost, h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                    });

                    cfg.Message<TeamCreatedEvent>(m =>
                    {
                        m.SetEntityName("team-created-exchange");
                    });
                });
            });

        return services;
    }
}
