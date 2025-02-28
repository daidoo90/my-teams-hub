namespace MyTeamsHub.Messaging.Models;

public class CreateNewTeamCommand : BaseCommand
{
    public string Name { get; set; }

    public CreateNewTeamCommand(string name)
    {
        Name = name;
    }
}
