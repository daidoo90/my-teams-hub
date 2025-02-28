using Microsoft.Extensions.DependencyInjection;

using MyTeamsHub.SignalR.Users.Service;

namespace MyTeamsHub.Organization.Notifications;

public static class DependencyInjection
{
    public static void AddSignalRServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserNotificationService, UserNotificationService>()
            .AddSignalR();
    }
}
