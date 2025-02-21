using Microsoft.AspNetCore.SignalR;

using MyTeamsHub.SignalR.Users.Events;
using MyTeamsHub.SignalR.Users.Hub;

namespace MyTeamsHub.SignalR.Users.Service;

public class UserNotificationService(IHubContext<UserNotificationHub, IUserNotificationHub> hubContext) : IUserNotificationService
{
    private readonly IHubContext<UserNotificationHub, IUserNotificationHub> _hubContext = hubContext;

    public async Task UserCreated(string email, string name)
    {
        await _hubContext.Clients.All.UserCreated(new UserCreatedEvent(email, name));
    }
}
