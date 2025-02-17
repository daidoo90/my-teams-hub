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
    public async Task SendAsync<T>(T data, CancellationToken token = default)
    {
        var messageEnvelope = new MessageEnvelope<T>(data);

        await _publishEndpoint.Publish(messageEnvelope, token);
    }
}
