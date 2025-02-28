using MassTransit;

using MyTeamsHub.Core.Application.Interfaces.Shared;

namespace MyTeamsHub.Infrastructure.Messaging;

public class MessageProducerService : IMessageProducerService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageProducerService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    /// <inheritdoc/>
    public async Task SendAsync<T>(T message, CancellationToken token = default)
    {
        await _publishEndpoint.Publish(message, token);
    }
}
