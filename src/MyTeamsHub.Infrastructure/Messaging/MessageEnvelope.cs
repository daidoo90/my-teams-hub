namespace MyTeamsHub.Infrastructure.Messaging;

public sealed record MessageEnvelope<T>
{
    public T Data { get; }

    public DateTimeOffset CreatedOn { get; }

    public MessageEnvelope(T data)
    {
        Data = data;
        CreatedOn = DateTimeOffset.UtcNow;
    }
}
