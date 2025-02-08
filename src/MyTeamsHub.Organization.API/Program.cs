using MyTeamsHub.Organization.API.Configurations;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    await builder
        .Build()
        .UseWebApiPipeline()
        .RunAsync();
}
catch (Exception exc)
{
    // TODO
}
finally
{
    // TODO
}
