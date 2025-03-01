using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MyTeamsHub.Core.Application;
using MyTeamsHub.Infrastructure;
using MyTeamsHub.Organization.Persistence;
using MyTeamsHub.Persistence.Registers;

var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;

                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                services.AddLogging(logging =>
                 {
                     logging.SetMinimumLevel(LogLevel.Debug);
                 });

                services.ConfigureDatabaseOptions(configuration);

                services
                    .AddApplication()
                    .AddInfrastructure(context.Configuration)
                    .AddPersistanceInfrastructure();
            })
            .Build();

await host.RunAsync();
