namespace MyTeamsHub.Organization.API.Configurations;

internal static class ControllersConfiguration
{
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        //services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddControllers();

        return services;
    }
}