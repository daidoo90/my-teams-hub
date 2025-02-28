using MyTeamsHub.SignalR.Users.Events;

namespace MyTeamsHub.SignalR.Users.Hub;

public interface IUserNotificationHub
{
    Task UserCreated(UserCreatedEvent user);
}
