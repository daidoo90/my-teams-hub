using Microsoft.AspNetCore.SignalR;

using MyTeamsHub.SignalR.Users.Events;

namespace MyTeamsHub.SignalR.Users.Hub;

public class UserNotificationHub : Hub<IUserNotificationHub>
{
    public async Task UserCreated(UserCreatedEvent user)
    {
        await Clients.All.UserCreated(user);
    }
}
