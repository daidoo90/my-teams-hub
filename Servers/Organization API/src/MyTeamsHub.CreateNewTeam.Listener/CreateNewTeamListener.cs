
using MassTransit;

using MyTeamsHub.Messaging.Models;

namespace MyTeamsHub.CreateNewTeam.Consumer;

public class CreateNewTeamListener : IConsumer<CreateNewTeamCommand>
{
    public Task Consume(ConsumeContext<CreateNewTeamCommand> context)
    {
        Console.WriteLine($"Received message: {context.Message.Name}");
        return Task.CompletedTask;
    }
}
