namespace MyTeamsHub.Messaging.Models;

public class BaseCommand
{
    public Guid MessageId { get; init; }

    public DateTimeOffset CreatedOn { get; init; }

    public BaseCommand()
    {
        MessageId = Guid.NewGuid();
        CreatedOn = DateTimeOffset.UtcNow;
    }
}
