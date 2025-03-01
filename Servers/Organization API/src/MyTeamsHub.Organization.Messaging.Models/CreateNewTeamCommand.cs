namespace MyTeamsHub.Messaging.Models;

public class CreateNewTeamCommand
{
    public string Name { get; set; }

    public Guid MessageId { get; init; }

    public DateTimeOffset CreatedOn { get; init; }

    public CreateNewTeamCommand(string name)
    {
        Name = name;
        MessageId = Guid.NewGuid();
        CreatedOn = DateTimeOffset.UtcNow;
    }
}
