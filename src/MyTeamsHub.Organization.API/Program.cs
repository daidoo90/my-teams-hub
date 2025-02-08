

using MyTeamsHub.Organization.API.Configurations;

try
{
    var builder = WebApplication.CreateBuilder(args);

    ApiConfiguration.ConfigureServices(builder);

    builder
        .Build()
        .UseWebApiPipeline()
        .Run();
}
catch (Exception exc)
{
    // TODO
}
finally
{
    // TODO
}
