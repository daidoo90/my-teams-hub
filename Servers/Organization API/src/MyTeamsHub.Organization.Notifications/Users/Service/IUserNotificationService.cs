namespace MyTeamsHub.SignalR.Users.Service;

public interface IUserNotificationService
{
    Task UserCreated(string email, string name);
}
