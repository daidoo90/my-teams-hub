using Microsoft.Extensions.DependencyInjection;
using MyTeamsHub.Persistence.Core.Context;
using MyTeamsHub.Persistence.Core.Repository;

namespace MyTeamsHub.Persistence.Core.Registers;

public static class RepositoryRegistrar
{
    /// <summary>
    /// Registers the repositories into the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TContext">The database context type.</typeparam>
    /// <param name="services">The service collection.</param>
    public static IServiceCollection AddRepositories<TContext>(this IServiceCollection services)
        where TContext : class, IDbContext
    {
        services.AddScoped<IDbContext, TContext>();
        services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IEfDeletableRepository<>), typeof(EfDeletableRepository<>));

        return services;
    }
}